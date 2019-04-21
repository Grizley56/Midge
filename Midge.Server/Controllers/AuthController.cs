using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Midge.Server.Communication;
using Midge.Server.Communication.Core;
using Midge.Server.Core;
using Newtonsoft.Json.Linq;

namespace Midge.Server.Controllers
{
	[MidgeController("auth")]
	public class AuthController: ControllerBase
	{
		[UsedImplicitly]
		[MidgeCommand("")]
		public async Task Authorize(
			[MidgeParameter("login")] string login,
			[MidgeParameter("password")] string password)
		{
			if (login == "admin" && password == "admin")
			{
				Context.UsersManager.SignIn(Context.CurrentConnection, new MidgeCredentials(login, password, Guid.NewGuid()));
				Response = new JProperty("result", "OK");
			}
			else
			{
				Response = new JProperty("result", "BAD");
			}
		}

		public AuthController(MidgeContext context, IServiceManager serviceManager) 
			: base(context, serviceManager)
		{
			
		}
	}
}
