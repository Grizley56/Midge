using Midge.Server.Web;

namespace Midge.Server.Communication
{
	public class MidgeContext
	{
		public MidgeContext(IClientConnection currentConnection, IMidgeUsersManager usersManager, IAudioBroadcaster audioBroadcaster)
		{
			CurrentConnection = currentConnection;
			UsersManager = usersManager;
			AudioBroadcaster = audioBroadcaster;
		}

		public IClientConnection CurrentConnection { get; }
		public IMidgeUsersManager UsersManager { get; }
		public IAudioBroadcaster AudioBroadcaster { get; }
	}
}