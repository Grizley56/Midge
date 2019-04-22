using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Midge.Server.Communication;
using Midge.Server.Communication.Core;
using Midge.Server.Communication.Parsers;
using Midge.Server.Controllers;
using Midge.Server.Core;
using Midge.Server.Extensions;
using Midge.Server.Services;
using Midge.Server.Web.Ssl;
using Midge.Server.Web.Tcp;
using Midge.Server.Web.Udp;
using Newtonsoft.Json.Linq;

namespace Midge.Server
{
	public class MidgeServer
	{
		public IPEndPoint Ip { get; }
		public X509Certificate Certificate { get; }

		public Encoding Encoding { get; set; } = Encoding.UTF8;

		public TcpSslServer InternalTcp { get; private set; }
		public UdpServer InternalUdp { get; private set; }

		private readonly MidgeControllerFactory _controllerFactory;
		private readonly IClientJsonMessageParser _clientMessageParser;
		private readonly IServerJsonMessageParser _serverMessageParser;

		public IMidgeUsersManager UsersManager { get; set; }
		public IAudioBroadcaster AudioBroadcaster { get; set; }
		public IDependencyStorage DependencyStorage { get; set; }
		public IServiceManager ServiceManager { get; set; }

		public MidgeServer(int port, X509Certificate certificate)
		{
			Ip = new IPEndPoint(IPAddress.Any, port);
			Certificate = certificate;

			InternalTcp = new TcpSslServer(Ip, Certificate);
			InternalUdp = new UdpServer(port + 1);

			UsersManager = new MidgeUsersManager(InternalTcp);
			AudioBroadcaster = new AudioBroadcaster(InternalUdp);
			ServiceManager = new MidgeServiceManager();

			_clientMessageParser = new ClientMessageParser();
			_serverMessageParser = new ServerMessageParser();
			_controllerFactory = new MidgeControllerFactory(new MidgeControllerParser());
		}

		private void InitializeServices()
		{
			ServiceManager = new MidgeServiceManager();

			if (DependencyStorage == null)
				return;

			ServiceManager.AddService(typeof(IProcessService), DependencyStorage.ProcessService);
			ServiceManager.AddService(typeof(IVolumeService), DependencyStorage.VolumeService);
			ServiceManager.AddService(typeof(IControlService), DependencyStorage.ControlService);
			ServiceManager.AddService(typeof(IAudioStreamService), DependencyStorage.AudioStreamService);
		}

		private void InternalServerMessageReceived(object sender, TcpMessageReceivedEventArgs e)
		{
			string message = Encoding.GetString(e.Message.GetMessage());

			ClientMessage clientMessage = _clientMessageParser.ParseFromJson(message);

			MidgeCommandInvoker commandInvoker = _controllerFactory.Create(clientMessage);

			commandInvoker.InvokeAsync(
				new MidgeContext(e.TcpClient, UsersManager, AudioBroadcaster), ServiceManager,
				(controller) =>
				{
					if (controller.Response == null)
						return;

					var response = _serverMessageParser.Parse(controller.Response, clientMessage.Key);

					e.TcpClient.SendMessage(new TcpMessage(response.ToString()));
					Debug.WriteLine("es! " + response.ToString());
				}, (controller, exception) =>
				{
					var response = _serverMessageParser.ParseError(exception.Message, clientMessage.Key);

					e.TcpClient.SendMessage(new TcpMessage(response.ToString()));
				});
		}

		public async Task Start()
		{
			if (UsersManager == null)
				throw new InvalidOperationException($"{nameof(UsersManager)} is null");

			if (DependencyStorage == null)
				throw new InvalidOperationException($"{nameof(DependencyStorage)} is null");

			if (ServiceManager == null)
				throw new InvalidOperationException($"{nameof(ServiceManager)} is null");

			InitializeServices();

			InternalTcp.MessageReceived += InternalServerMessageReceived;

			await InternalTcp.StartAsync();
			AudioBroadcaster.Start();
		}

		public async Task Stop()
		{
			InternalTcp.MessageReceived -= InternalServerMessageReceived;

			await InternalTcp.StopAsync();
			AudioBroadcaster.Stop();
		}
	}
}
