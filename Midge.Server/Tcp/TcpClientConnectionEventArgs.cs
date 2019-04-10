using System;

namespace Midge.Server.Tcp
{
	public class TcpClientConnectionEventArgs: EventArgs
	{
		public readonly ITcpClientConnection TcpClient;

		public TcpClientConnectionEventArgs(ITcpClientConnection tcpClient)
		{
			TcpClient = tcpClient;
		}
	}
}
