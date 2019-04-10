using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Midge.Server.Tcp;

namespace Midge.Server.Ssl
{
	public interface ITcpClientSslConnection: ITcpClientConnection
	{
		bool IsProtected { get; }
		bool Authorize(X509Certificate certificate);
		Task<bool> AuthorizeAsync(X509Certificate certificate);
	}
}
