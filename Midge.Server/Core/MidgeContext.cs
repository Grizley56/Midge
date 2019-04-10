using System.Collections.Generic;

namespace Midge.Server.Core
{
	public class MidgeContext: IContext
	{
		public MidgeContext(IEnumerable<MidgeUser> users, MidgeUser currentUser)
		{
			Users = users;
			CurrentUser = currentUser;
		}

		public IEnumerable<MidgeUser> Users { get; }
		public MidgeUser CurrentUser { get; }
	}
}