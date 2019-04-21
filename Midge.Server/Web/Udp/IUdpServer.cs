using System.Net;
using System.Threading.Tasks;

namespace Midge.Server.Web.Udp
{
	public interface IUdpServer: IServer
	{
		Task Send(IPEndPoint ip, byte[] data);
	}
}
