using System;

namespace Midge.Server
{
	public class MidgeUser
	{
		public string UserName { get; }
		public DateTime SignInDateTime { get; }

		public bool IsAuthorized { get; set; } = false;

		private bool _isUnknown;

		public MidgeUser(string userName, DateTime signInDateTime)
		{
			UserName = UserName;
			SignInDateTime = signInDateTime;
		}

		protected MidgeUser()
		{
			_isUnknown = true;
		}

		public static MidgeUser Unknown => new MidgeUser();
	}
}
