using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Midge.Server.Web.Tcp;

namespace Midge.Server.Web.Ssl
{
	public class TcpClientSslConnection: TcpClientConnection, ITcpClientSslConnection
	{
		private SslStream ReadWriteSslStream => (SslStream) ReadWriteStream;

		public TcpClientSslConnection(TcpClient tcpClient)
			: base(tcpClient)
		{
			ReadWriteStream = new SslStream(NetworkStream, false);
		}


		public bool IsProtected
		{
			get
			{
				var sslStream = (SslStream) ReadWriteStream;
				return sslStream.IsAuthenticated;
			}
		}

		public bool Authorize(X509Certificate certificate)
		{
			if (IsProtected)
				return true;

			try
			{
				ReadWriteSslStream.AuthenticateAsServer(certificate, false, SslProtocols.Tls12, false);
			}
			catch (AuthenticationException)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> AuthorizeAsync(X509Certificate certificate)
		{
			if (IsProtected)
				return true;

			try
			{
				await ReadWriteSslStream.AuthenticateAsServerAsync(certificate, false, SslProtocols.Tls12, false);
			}
			catch (AuthenticationException)
			{
				return false;
			}

			return true;
		}
	}
}
