using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Midge.Server.Extensions;

namespace Midge.Server.Tcp
{
	public class TcpServer : ITcpServer, IDisposable
	{
		private readonly TcpListener _listener;
		private readonly ConcurrentDictionary<Guid, TcpClientConnection> _connections;
		private CancellationTokenSource _internalTokenSource;

		public IReadOnlyCollection<ITcpClientConnection> Connections => _connections.Values.AsReadOnly();

		public IPEndPoint Ip { get; }
		
		public event EventHandler<TcpMessageReceivedEventArgs> MessageReceived;
		public event EventHandler<TcpClientConnectionEventArgs> ConnectionOpened;
		public event EventHandler<TcpClientConnectionEventArgs> ConnectionClosed;

		public event EventHandler<TcpServerStateEventArgs> ServerStateChanged;

		public int MaxConnectionsCount { get; }
		public bool ReadConnections { get; set; }

		public bool AcceptConnections { get; set; } = true;

		private Task _mainTask;

		public TcpServer(IPEndPoint ipEndPoint, int maxConnections = 100)
		{
			Ip = ipEndPoint;
			MaxConnectionsCount = maxConnections;

			_listener = new TcpListener(Ip);
			_connections = new ConcurrentDictionary<Guid, TcpClientConnection>();

			State = TcpServerState.Stopped;
		}

		private readonly object _stateLock = new object();
		private TcpServerState _state;

		public async Task<bool> StartAsync()
		{
			lock (_stateLock)
			{
				if (State != TcpServerState.Stopped)
					return false;

				State = TcpServerState.Starting;
			}


			_internalTokenSource = new CancellationTokenSource();
			_listener.Start(50);

			_mainTask = ReceiveConnections(_internalTokenSource.Token);

			lock (_stateLock)
				State = TcpServerState.Started;

			return true;
		}

		public async Task<bool> StopAsync()
		{
			lock (_stateLock)
			{
				if (State != TcpServerState.Started)
					return false;

				State = TcpServerState.Stopping;
			}

			_internalTokenSource.Cancel();

			await _mainTask;

			foreach (var connection in _connections.Values)
				connection.Disconnect();

			_connections.Clear();

			lock (_stateLock)
			{
				State = TcpServerState.Stopped;
			}

			return true;
		}

		private async Task ReceiveConnections(CancellationToken token)
		{
			try
			{
				while (!token.IsCancellationRequested)
				{
					TcpClient client;

					try
					{
						client = await _listener.AcceptTcpClientAsync(token);
					}
					catch (OperationCanceledException)
					{
						break;
					}

					if (_connections.Count == MaxConnectionsCount)
					{
						client.Close();
						continue;
					}

					TcpClientConnection newConnection = ProcessConnection(client);
					Task.Factory.StartNew(() => StartListenClient(newConnection, token), TaskCreationOptions.LongRunning);
				}
			}
			catch
			{
				;
			}
			finally
			{
				_listener.Stop();
				OnServerStateChanged(new TcpServerStateEventArgs(State));
			}
		}

		protected virtual TcpClientConnection ProcessConnection(TcpClient client)
		{
			return new TcpClientConnection(client);
		}

		private void StartListenClient(TcpClientConnection connection, CancellationToken token)
		{
			_connections.TryAdd(connection.ConnectionId, connection);
			OnConnectionOpened(new TcpClientConnectionEventArgs(connection));

			try
			{
				while (!token.IsCancellationRequested)
				{
					if (!connection.IsConnected())
						break;

					ITcpMessage message;

					try
					{
						message = connection.ReadMessage();
					}
					catch (OperationCanceledException)
					{
						return;
					}
					catch (Exception)
					{
						connection.Disconnect();
						OnConnectionClosed(new TcpClientConnectionEventArgs(connection));
						break;
					}

					OnMessageReceived(new TcpMessageReceivedEventArgs(connection, message));
				}
			}
			finally
			{
				_connections.TryRemove(connection.ConnectionId, out var _);
			}
		}

		public TcpServerState State
		{
			get => _state;
			private set
			{
				if (_state == value)
					return;

				_state = value;
				OnServerStateChanged(new TcpServerStateEventArgs(value));
			}
		}
		
		protected virtual void OnServerStateChanged(TcpServerStateEventArgs e)
		{
			ServerStateChanged?.Invoke(this, e);
		}

		protected virtual void OnConnectionOpened(TcpClientConnectionEventArgs e)
		{
			ConnectionOpened?.Invoke(this, e);
		}

		protected virtual void OnConnectionClosed(TcpClientConnectionEventArgs e)
		{
			ConnectionClosed?.Invoke(this, e);
		}

		protected virtual void OnMessageReceived(TcpMessageReceivedEventArgs e)
		{
			MessageReceived?.Invoke(this, e);
		}

		public void Dispose()
		{
			_internalTokenSource?.Dispose();
			_mainTask?.Dispose();
		}
	}
}