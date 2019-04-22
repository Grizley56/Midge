using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Midge.API.Categories;
using Midge.API.Core;
using Midge.API.EventArgs;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Midge.API
{
	public class MidgeClient : IMidgeInvoke
	{
		public AudioCategory Audio { get; private set; }
		public ProcessCategory Process { get; private set; }
		public ControlCategory Control { get; private set; }
		public FileManagerCategory FileManager { get; private set; }

		public MidgeRestClient RestClient { get; private set; }

		private readonly ConcurrentDictionary<Guid, IWaiter<JToken>> _waiters;

		//private string _accessToken;

		private JsonSerializer _jsonSerializer;

		public MidgeClient()
		{
			RestClient = new MidgeRestClient(Encoding.UTF8);

			RestClient.ServerMessageReceived += ServerMessageReceived;
			
			_waiters = new ConcurrentDictionary<Guid, IWaiter<JToken>>();

			Initialize();
		}

		private void Initialize()
		{
			Audio = new AudioCategory(this);
			Process = new ProcessCategory(this);
			Control = new ControlCategory(this);
			FileManager = new FileManagerCategory(this);

			_jsonSerializer = new JsonSerializer();
			_jsonSerializer.Converters.Add(new UnixDateTimeConverter());
			_jsonSerializer.Converters.Add(new StringEnumConverter());
		}

		private void ServerMessageReceived(object sender, MidgeMessageReceivedEventArgs e)
		{
			try
			{
				var json = JObject.Parse(e.Message);

				Guid? commandToken = json?["command_token"]?.ToObject<Guid>();

				if (commandToken == null)
					return;
				
				if (!_waiters.ContainsKey(commandToken.Value))
					return;

				_waiters.TryRemove(commandToken.Value, out var waiter);

				waiter.SetReady(json["response"]);
			}
			catch
			{
				;
			}
		}

		public async Task Run(IPEndPoint ip)
		{
			await RestClient.Start(ip);
		}

		public async Task Stop()
		{
			await RestClient.Stop();
		}

		public Task SendAsync(string method, MidgeParameters parameters)
		{
			var request = CreateRequest(method, parameters);

			return RestClient.SendMessageAsync(request.ToString());
		}

		public async Task<T> SendAndWaitAsync<T>(string methodName, MidgeParameters parameters, int timeout = 100000)
		{
			Guid commandToken = Guid.NewGuid();

			var request = CreateRequest(methodName, parameters, commandToken);
			
			ResponseWaiter waiter = new ResponseWaiter(commandToken);

			_waiters.TryAdd(commandToken, waiter);

			await RestClient.SendMessageAsync(request.ToString());

			await waiter.WaitAsync(timeout);

			var result = waiter.Result.ToObject<T>(_jsonSerializer);

			return result;
		}

		public void Send(string methodName, MidgeParameters parameters)
		{
			var request = CreateRequest(methodName, parameters);

			RestClient.SendMessage(request.ToString());
		}

		public T SendAndWait<T>(string method, MidgeParameters parameters, int timeout = 100000)
		{
			Guid commandToken = Guid.NewGuid();

			var request = CreateRequest(method, parameters, commandToken);

			ResponseWaiter waiter = new ResponseWaiter(commandToken);

			_waiters.TryAdd(commandToken, waiter);

			RestClient.SendMessage(request.ToString());

			waiter.Wait(timeout);

			var result = waiter.Result.ToObject<T>(_jsonSerializer);

			return result;
		}

		public JObject CreateRequest(string methodName, MidgeParameters @params, Guid? commandToken = null)
		{
			var request = new JObject(
				new JProperty("method", methodName));

			if (@params.Count > 0)
				request.Add(
					new JProperty("params",
						new JObject(from param in @params select new JProperty(param.Key, param.Value))));

			if (commandToken != null)
				request.Add(
					new JProperty("command_token", commandToken.Value));

			return request;
		}
	}
}
