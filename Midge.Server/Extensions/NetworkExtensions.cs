using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Midge.Server.Extensions
{
	public static class NetworkExtensions
	{
		public static async Task<TcpClient> AcceptTcpClientAsync(this TcpListener listener, CancellationToken token)
		{
			token.Register(listener.Stop);

			try
			{
				return await listener.AcceptTcpClientAsync();
			}
			catch (Exception ex) when (token.IsCancellationRequested)
			{
				throw new OperationCanceledException("Cancellation was requested while awaiting TCP client connection.", ex);
			}
		}

		public static bool IsConnected(this Socket socket)
		{
			if (socket == null || !socket.Connected || socket.Poll(1000, SelectMode.SelectRead) && socket.Available == 0)
				return false;

			bool blockingState = socket.Blocking;

			try
			{
				byte[] ping = new byte[1];
				socket.Blocking = false;
				socket.Send(ping, 0, 0);

				return true;
			}
			catch (SocketException ex)
			{
				// 10035 == WSAEWOULDBLOCK (still connected, but send would block)
				return ex.NativeErrorCode.Equals(10035);
			}
			finally
			{
				socket.Blocking = blockingState;
			}
		}
	}
}
