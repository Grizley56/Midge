using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Midge.Server.Web.Udp
{
	public class UdpServer: IUdpServer
	{
		private readonly UdpClient _udpClient;

		public UdpServer(int port)
		{
			Ip = new IPEndPoint(IPAddress.Any, port);

			_udpClient = new UdpClient(Ip.Port);
		}


		public Task Send(IPEndPoint ip, byte[] data)
		{
			return _udpClient.SendAsync(data, data.Length, ip);
		}

		public IPEndPoint Ip { get; }

		public event EventHandler<ServerStateEventArgs> ServerStateChanged;

		public ServerState State { get; } = ServerState.Started;

		public async Task<bool> StartAsync()
		{
			return true;
		}

		public async Task<bool> StopAsync()
		{
			return true;
		}
	}
}
