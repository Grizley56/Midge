using System;
using System.ServiceModel;
using System.Threading;
using MidgeContract;

namespace Midge.Core
{
	public class ReconnectableChannel<TChannel>
		where TChannel : class, IConnectionCheckable
	{
		private ChannelFactory<TChannel> _processorFactory;

		private static ReconnectableChannel<IMidge> _instance;
		public static ReconnectableChannel<IMidge> MidgeInstance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ReconnectableChannel<IMidge>();
					_instance.Start();
				}

				return _instance;
			}
		}

		public event EventHandler<EventArgs> ChannelStateChanged;

		private TChannel _contract;
		private TimeSpan _reconnectionFrequency;

		private readonly Timer _tryProcessOpenTimer;
		
		private CommunicationState _state = CommunicationState.Created;

		public CommunicationState State
		{
			get => _state;
			set
			{
				if (_state == value)
					return;

				_state = value;
				OnChannelStateChanged();
			}
		}

		protected TChannel Contract
		{
			get => State == CommunicationState.Opened || ClientChannel.State == CommunicationState.Created ? _contract : null;
			set => _contract = value;
		}

		protected IClientChannel ClientChannel => (IClientChannel) _contract;

		public ReconnectableChannel()
		{
			_tryProcessOpenTimer = new Timer(i => { OpenChannelTimerTick(); });
		}

		public bool Invoke(Action<TChannel> func)
		{
			if (Contract == null)
				return false;

			try
			{
				func?.Invoke(Contract);
			}
			catch (Exception ex) when (ex is CommunicationException || ex is TimeoutException)
			{
				ClientChannel.Close();
				return false;
			}

			return true;
		}

		private void SubscribeToChannelEvents()
		{
			if (ClientChannel == null)
				return;

			ClientChannel.Opening += ClientChannelChanged;
			ClientChannel.Opened += ClientChannelChanged;
			ClientChannel.Closing += ClientChannelChanged;
			ClientChannel.Closed += ClientChannelChanged;
			ClientChannel.Faulted += ClientChannelChanged;
		}


		private void UnsubscribeToChannelEvents()
		{
			if (ClientChannel == null)
				return;

			ClientChannel.Opening -= ClientChannelChanged;
			ClientChannel.Opened -= ClientChannelChanged;
			ClientChannel.Closing -= ClientChannelChanged;
			ClientChannel.Closed -= ClientChannelChanged;
			ClientChannel.Faulted -= ClientChannelChanged;
		}

		public bool OpenChannel()
		{
			try
			{
				UnsubscribeToChannelEvents();

				_processorFactory = new ChannelFactory<TChannel>(new BasicHttpBinding()
				{
					OpenTimeout = TimeSpan.FromSeconds(5),
					ReceiveTimeout = TimeSpan.FromSeconds(5),
					SendTimeout = TimeSpan.FromSeconds(5)
				}, new EndpointAddress($"http://{Settings.IpAddress}:{Settings.Port}/Midge"));

				Contract = _processorFactory.CreateChannel();
				State = CommunicationState.Created;
				Contract.IsConnected();
				State = CommunicationState.Opened;
			}
			catch
			{
				ClientChannel.Close();
				State = CommunicationState.Faulted;
				return false;
			}

			SubscribeToChannelEvents();
			return true;
		}

		private void OpenChannelTimerTick()
		{
			if (State != CommunicationState.Opened)
			{
				OpenChannel();
			}
		}

		private void ClientChannelChanged(object sender, EventArgs args)
		{
			State = ((IClientChannel) sender).State;
		}

		public void Start()
		{
			_reconnectionFrequency = TimeSpan.FromMilliseconds(Settings.ReconnectionTimeout);
			_tryProcessOpenTimer.Change(TimeSpan.Zero, _reconnectionFrequency);
		}

		public void Stop()
		{
			_tryProcessOpenTimer.Change(Timeout.Infinite, Timeout.Infinite);
			
			_processorFactory.Close();
		}

		public void Restart()
		{
			Stop();
			Start();
		}

		protected virtual void OnChannelStateChanged()
		{
			ChannelStateChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
