namespace Midge.Server.Tcp
{
	public class TcpServerStateEventArgs
	{
		public readonly TcpServerState State;

		public TcpServerStateEventArgs(TcpServerState state)
		{
			State = state;
		}
	}
}
