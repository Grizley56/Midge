using System;

namespace Midge.Server.Tcp
{
	public class TcpMessageReceivedEventArgs: EventArgs
	{
		public readonly ITcpMessage Message;
		public readonly ITcpClientConnection TcpClient;

		public TcpMessageReceivedEventArgs(ITcpClientConnection client, ITcpMessage message)
		{
			Message = message;
			TcpClient = client;
		}
	}
}
