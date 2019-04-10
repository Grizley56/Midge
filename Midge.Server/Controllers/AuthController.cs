using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Midge.Server.Core;

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
			
		}

		public AuthController(IContext context, IServiceManager serviceManager) 
			: base(context, serviceManager)
		{
		}
	}
}
