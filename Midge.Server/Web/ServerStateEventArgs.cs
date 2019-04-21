namespace Midge.Server.Web
{
	public class ServerStateEventArgs
	{
		public readonly ServerState State;

		public ServerStateEventArgs(ServerState state)
		{
			State = state;
		}
	}
}
