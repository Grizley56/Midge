using System;
using System.Collections.Generic;
using System.Text;
using Midge.Server.Communication;
using Midge.Server.Communication.Core;
using Midge.Server.Core;
using Midge.Server.Services;

namespace Midge.Server.Controllers
{
	[MidgeController("system")]
	public class SystemController: ControllerBase
	{
		private ISystemService _system;

		public SystemController(MidgeContext context, IServiceManager serviceManager) : base(context, serviceManager)
		{
			_system = Services.GetService<ISystemService>();
		}

		[MidgeCommand("off")]
		public void TurnOff([MidgeParameter("delay")] int seconds)
		{
			_system.TurnOff(seconds);
		}

		[MidgeCommand("restart")]
		public void Restart([MidgeParameter("delay")] int seconds)
		{
			_system.Restart(seconds);
		}
	}
}
