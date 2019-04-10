using System.Security.Cryptography.X509Certificates;
using Midge.Server.Tcp;

namespace Midge.Server.Ssl
{
	public interface ITcpSslServer: ITcpServer
	{
		X509Certificate Certificate { get; }
	}
}
