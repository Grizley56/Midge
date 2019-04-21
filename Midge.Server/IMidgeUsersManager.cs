using System;
using System.Collections.Generic;
using System.Text;
using Midge.Server.Web;

namespace Midge.Server
{
	public interface IMidgeUsersManager
	{
		event EventHandler<MidgeUserEventArgs> UserSignIn;
		event EventHandler<MidgeUserEventArgs> UserSignOut;
		bool SignIn(IClientConnection connection, MidgeCredentials userCredentials);
		bool SignOut(IClientConnection connection);
		bool IsAuthorized(IClientConnection connection);

		IReadOnlyCollection<MidgeUser> OnlineUsers { get; }
	}
}
