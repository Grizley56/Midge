using System;
using System.Threading.Tasks;

namespace Midge.Server.Tcp
{
	public interface ITcpServer
	{
		event EventHandler<TcpMessageReceivedEventArgs> MessageReceived;
		event EventHandler<TcpClientConnectionEventArgs> ConnectionOpened;
		event EventHandler<TcpClientConnectionEventArgs> ConnectionClosed;

		Task<bool> StartAsync();
		Task<bool> StopAsync();

		TcpServerState State { get; }
		bool AcceptConnections { get; set; }
		bool ReadConnections { get; set; }

		event EventHandler<TcpServerStateEventArgs> ServerStateChanged;

		int MaxConnectionsCount { get; }
	}
}