using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Midge.Server.Tcp
{
	public interface ITcpClientConnection: IDisposable
	{
		Task SendMessageAsync(ITcpMessage message, CancellationToken token);
		void SendMessage(ITcpMessage message);

		Task<ITcpMessage> ReadMessageAsync(CancellationToken token);
		ITcpMessage ReadMessage();

		IReadOnlyCollection<ITcpMessage> MessagesInQueue { get; }
		IPAddress Ip { get; }

		void Disconnect();

		bool IsConnected();
	}
}