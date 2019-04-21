using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Midge.Server.Web
{
	public interface IServer
	{
		IPEndPoint Ip { get; }
		event EventHandler<ServerStateEventArgs> ServerStateChanged;
		ServerState State { get; }

		Task<bool> StartAsync();
		Task<bool> StopAsync();
	}
}
