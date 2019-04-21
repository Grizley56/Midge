using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Midge.Server.Web.Tcp
{
	public interface ITcpServer: IServer
	{
		event EventHandler<TcpMessageReceivedEventArgs> MessageReceived;
		event EventHandler<TcpClientConnectionEventArgs> ConnectionOpened;
		event EventHandler<TcpClientConnectionEventArgs> ConnectionClosed;

		bool ReadConnections { get; set; }
		int MaxConnectionsCount { get; }
		bool AcceptConnections { get; set; }

		IReadOnlyCollection<ITcpClientConnection> Connections { get; }
	}
}