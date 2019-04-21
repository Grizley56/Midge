using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Midge.Server.Web
{
	public interface IClientConnection
	{
		IPAddress Ip { get; }

		void Disconnect();

		bool IsConnected();
	}
}
