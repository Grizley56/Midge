using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Midge.API;

namespace Midge.Client.Mobile.Core
{
	public class MidgeCore
	{
		private static MidgeCore _instance;

		public static MidgeCore Instance
		{
			get
			{
				if (_instance == null)
					_instance = new MidgeCore();

				return _instance;
			}
		}

		public double ReconnectionTimeout
		{
			get => _connectTimer.Interval;
			set => _connectTimer.Interval = value;
		}

		public MidgeClient Client { get; private set; }

		public event EventHandler<EventArgs> ConnectionStateChanged;

		private readonly Timer _connectTimer = new Timer();

		private MidgeCore()
		{
			Client = new MidgeClient();
			Client.RestClient.Faulted += (s, a) => State = ConnectionState.Disconnected;
			Client.RestClient.Stopped += (s, a) => State = ConnectionState.Disconnected;
			Client.RestClient.Started += (s, a) => State = ConnectionState.Connected;

			_connectTimer.Elapsed += ConnectTimerElapsed;
		}

		private async void ConnectTimerElapsed(object sender, ElapsedEventArgs e)
		{
			if (State == ConnectionState.Connected)
				return;

			await TryConnect();
		}

		private async Task TryConnect()
		{
			IPEndPoint address = Settings.ServerAddress;

			if (address == null)
				return;

			try
			{
				await Client.Run(address);
			}
			catch
			{
				Debug.WriteLine("Connect failed.");
			}
		}

		public async Task Start()
		{
			await TryConnect();

			_connectTimer.Start();
		}

		public void Stop()
		{
			_connectTimer.Stop();
		}

		public Task Restart()
		{
			Stop();
			return Start();
		}

		private ConnectionState _state = ConnectionState.Disconnected;

		public ConnectionState State
		{
			get => _state;
			private set
			{
				if (_state == value)
					return;

				_state = value;
				OnConnectionStateChanged();
			}
		}


		protected virtual void OnConnectionStateChanged()
		{
			ConnectionStateChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
