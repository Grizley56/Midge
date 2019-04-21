using System.Security.Cryptography.X509Certificates;
using Midge.Server.Web.Tcp;

namespace Midge.Server.Web.Ssl
{
	public interface ITcpSslServer: ITcpServer
	{
		X509Certificate Certificate { get; }
	}
}
