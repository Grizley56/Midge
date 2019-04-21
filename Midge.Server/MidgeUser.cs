using System;
using System.Net;

namespace Midge.Server
{
	public class MidgeUser
	{
		public MidgeCredentials Credentials { get; }
		public DateTime SignInDateTime { get; }
		public IPAddress Ip { get; }

		public MidgeUser(MidgeCredentials credentials, IPAddress ip, DateTime signInDateTime)
		{
			Credentials = credentials;
			Ip = ip;
			SignInDateTime = signInDateTime;
		}



	}
}
