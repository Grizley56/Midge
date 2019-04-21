using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using Midge.Server.Web.Tcp;

namespace Midge.Server.Web.Ssl
{
	public class TcpSslServer : TcpServer, ITcpSslServer
	{
		public TcpSslServer(IPEndPoint ipEndPoint, X509Certificate cert)
			: base(ipEndPoint)
		{
			Certificate = cert;
		}


		public X509Certificate Certificate { get; }

		protected override TcpClientConnection ProcessConnection(TcpClient client)
		{
			var connection = new TcpClientSslConnection(client);

			if (connection.Authorize(Certificate))
				return connection;

			throw new System.Exception($"Failed to authorize tcp connection {connection}");
		}
	}
}
