using System;

namespace Midge.Server
{
	public class MidgeUserEventArgs: EventArgs
	{
		public readonly MidgeUser User;

		public MidgeUserEventArgs(MidgeUser user)
		{
			User = user;
		}
	}
}