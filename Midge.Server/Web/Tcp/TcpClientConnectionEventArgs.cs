using System;

namespace Midge.Server.Web.Tcp
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
