using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Midge.Server.Controllers;
using Midge.Server.Core;
using Midge.Server.Extensions;
using Midge.Server.Services;
using Midge.Server.Ssl;
using Midge.Server.Tcp;
using Newtonsoft.Json.Linq;

namespace Midge.Server
{
	public class MidgeServer
	{
		public IPEndPoint Ip { get; }
		public X509Certificate Certificate { get; }

		public Encoding Encoding { get; set; } = Encoding.UTF8;

		public TcpSslServer InternalServer { get; private set; }

		public IReadOnlyCollection<MidgeUser> OnlineUsers => _onlineUsers.Values.AsReadOnly();

		private readonly ConcurrentDictionary<ITcpClientConnection, MidgeUser> _onlineUsers;

		public IJsonMessageParser MessageParser { get; set; }
		public IControllerParser ControllerParser { get; set; }
		public IDependencyStorage DependencyStorage { get; set; }
		public IServiceManager ServiceManager { get; set; }
			
		public MidgeServer(int port, X509Certificate certificate)
		{
			Ip = new IPEndPoint(IPAddress.Any, port);
			Certificate = certificate;

			InternalServer = new TcpSslServer(Ip, Certificate);

			InternalServer.ConnectionOpened += InternalServerConnectionOpened;
			InternalServer.ConnectionClosed += InternalServerConnectionClosed;
			InternalServer.MessageReceived += InternalServerMessageReceived;
			_onlineUsers = new ConcurrentDictionary<ITcpClientConnection, MidgeUser>();

			MessageParser = new JsonMessageParser();
			ControllerParser = new MidgeControllerParser();
			ServiceManager = new MidgeServiceManager();
		}

		private void InitializeServices()
		{
			ServiceManager = new MidgeServiceManager();

			if (DependencyStorage == null)
				return;

			ServiceManager.AddService(typeof(IProcessService), DependencyStorage.ProcessService);
			ServiceManager.AddService(typeof(IVolumeService), DependencyStorage.VolumeService);
		}

		private void InternalServerMessageReceived(object sender, TcpMessageReceivedEventArgs e)
		{
			if (!_onlineUsers.TryGetValue(e.TcpClient, out var midgeUser))
				return;

			string message = Encoding.GetString(e.Message.GetMessage());
			JsonMessage jsonMessage = MessageParser.Parse(message);

			if (!ControllerParser.Supports(jsonMessage.Controller))
				return;

			MidgeControllerInfo controllerInfo = ControllerParser.GetController(jsonMessage.Controller);

			MidgeCommandInfo commandInfo = controllerInfo.GetCommandByName(jsonMessage.Command);

			if (commandInfo == null)
				return;

			var controllerInstance = (ControllerBase)Activator.CreateInstance(controllerInfo.ControllerType,
				new object[]
				{
					new MidgeContext(_onlineUsers.Values, midgeUser),
					ServiceManager
				});


			List<object> args = new List<object>();

			foreach (var param in commandInfo.Params.OrderBy(i => i.Order))
			{
				var userParam = jsonMessage.Parameters.FirstOrDefault(i =>
					i.Name.Equals(param.Name, StringComparison.OrdinalIgnoreCase));

				if (userParam == null && param.IsRequired)
					return; //error

				if (userParam == null)
				{
					args.Add(null);
					continue;
				}

				try
				{
					var arg = userParam.Value.ToObject(param.ParamType);
					args.Add(arg);
				}
				catch (Exception ex)
				{
					return; // error
				}
			}

			Task t;

			if (commandInfo.IsAsync)
				t = Task.Run(() => (Task)commandInfo.MethodInfo.Invoke(controllerInstance, args.ToArray()));
			else
				t = Task.Run(() => commandInfo.MethodInfo.Invoke(controllerInstance, args.ToArray()));

			t.ContinueWith(_ =>
			{
				if (controllerInstance.Response == null)
					return;

				var completeResponse = new JObject(
					new JProperty("response", controllerInstance.Response));

				if (jsonMessage.Key != null)
					completeResponse.Add(
						new JProperty("command_token", jsonMessage.Key.Value));

				e.TcpClient.SendMessage(new TcpMessage(completeResponse.ToString()));
			});
			
		}

		private void InternalServerConnectionClosed(object sender, TcpClientConnectionEventArgs e)
		{
			_onlineUsers.TryRemove(e.TcpClient, out var _);
		}

		private void InternalServerConnectionOpened(object sender, TcpClientConnectionEventArgs e)
		{
			_onlineUsers.TryAdd(e.TcpClient, MidgeUser.Unknown);
		}

		public async Task Start()
		{
			InitializeServices();

			await InternalServer.StartAsync();
		}

		public async Task Stop()
		{
			await InternalServer.StopAsync();
		}
	}
}
