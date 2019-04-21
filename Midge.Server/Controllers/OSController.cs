using System;
using System.Collections.Generic;
using System.Text;
using Midge.Server.Communication;
using Midge.Server.Communication.Core;
using Midge.Server.Core;
using Midge.Server.Services;

namespace Midge.Server.Controllers
{
	[MidgeController("os")]
	public class OSController: ControllerBase
	{
		private readonly IOperationSystemService _osService;

		public OSController(MidgeContext context, IServiceManager serviceManager) : base(context, serviceManager)
		{
			_osService = Services.GetService<IOperationSystemService>();
		}

		[MidgeCommand("turnOff")]
		public void TurnOff([MidgeParameter("delay", false)] int? ms = null )
		{
			_osService.TurnOff(ms ?? 0);
		}

		[MidgeCommand("restart")]
		public void Restart([MidgeParameter("delay", false)] int? ms = null)
		{
			_osService.Restart(ms ?? 0);
		}
	}
}
