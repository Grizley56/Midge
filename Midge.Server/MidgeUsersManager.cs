using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using JetBrains.Annotations;
using Midge.Server.Web;
using Midge.Server.Web.Tcp;

namespace Midge.Server
{
	public class MidgeUsersManager: IMidgeUsersManager
	{
		private readonly ConcurrentDictionary<IClientConnection, MidgeUser> _onlineUsers;
		private readonly ITcpServer _server;

		public event EventHandler<MidgeUserEventArgs> UserSignIn;
		public event EventHandler<MidgeUserEventArgs> UserSignOut;

		

		public MidgeUsersManager([NotNull] ITcpServer server)
		{
			_server = server;
			_onlineUsers = new ConcurrentDictionary<IClientConnection, MidgeUser>();
			server.ConnectionOpened += ServerConnectionOpened;
			server.ConnectionClosed += ServerConnectionClosed;

			Init();
		}

		private void Init()
		{
			foreach (var connection in _server.Connections)
			{
				_onlineUsers.TryAdd(connection, null);
			}
		}

		private void ServerConnectionClosed(object sender, TcpClientConnectionEventArgs e)
		{
			if(_onlineUsers.TryRemove(e.TcpClient, out var user) && user != null)
				OnUserSignOut(new MidgeUserEventArgs(user));
		}

		private void ServerConnectionOpened(object sender, TcpClientConnectionEventArgs e)
		{
			_onlineUsers.TryAdd(e.TcpClient, null);
		}

		public bool SignIn(IClientConnection connection, MidgeCredentials userCredentials)
		{
			if (_onlineUsers.TryGetValue(connection, out var user))
			{
				if (user != null)
					OnUserSignOut(new MidgeUserEventArgs(user));

				var newUser = new MidgeUser(userCredentials, connection.Ip, DateTime.Now);

				_onlineUsers[connection] = newUser;
				OnUserSignIn(new MidgeUserEventArgs(newUser));

				return true;
			}

			return false;
		}

		public bool SignOut(IClientConnection connection)
		{
			if (_onlineUsers.TryRemove(connection, out var user))
			{
				OnUserSignOut(new MidgeUserEventArgs(user));
				return true;
			}

			return false;
		}

		public bool IsAuthorized(IClientConnection connection)
		{
			if (_onlineUsers.TryGetValue(connection, out var user) && user != null)
				return true;

			return false;
		}

		public IReadOnlyCollection<MidgeUser> OnlineUsers
		{
			get
			{
				Collection<MidgeUser> users = new Collection<MidgeUser>();
				foreach(var i in _onlineUsers)
					if (i.Value != null)
						users.Add(i.Value);

				return users;
			}
		}

		protected virtual void OnUserSignIn(MidgeUserEventArgs e)
		{
			UserSignIn?.Invoke(this, e);
		}

		protected virtual void OnUserSignOut(MidgeUserEventArgs e)
		{
			UserSignOut?.Invoke(this, e);
		}
	}
}
