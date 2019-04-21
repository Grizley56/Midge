using System;
using System.Collections.Generic;
using System.Text;

namespace Midge.Server
{
	public class MidgeCredentials
	{
		public string Login { get; }
		public string Password { get; }
		public Guid PrivateKey { get; }

		public MidgeCredentials(string login, string password, Guid privateKey)
		{
			Login = login;
			Password = password;
			PrivateKey = privateKey;
		}
	}
}
