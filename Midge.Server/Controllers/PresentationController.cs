using System;
using System.Collections.Generic;
using System.Text;
using Midge.Server.Communication;
using Midge.Server.Communication.Core;
using Midge.Server.Core;
using Midge.Server.Services;

namespace Midge.Server.Controllers
{
	[MidgeController("presentation")]
	public class PresentationController: ControllerBase
	{
		private readonly IPresentationService _presentationService;

		public PresentationController(MidgeContext context, IServiceManager serviceManager) 
			: base(context, serviceManager)
		{
			_presentationService = Services.GetService<IPresentationService>();
		}

		[MidgeCommand("nextSlide")]
		public void NextSlide()
		{
			_presentationService.MoveNext();
		}

		[MidgeCommand("previousSlide")]
		public void PrevSlide()
		{
			_presentationService.MovePrevious();
		}

		[MidgeCommand("play")]
		public void Play()
		{
			_presentationService.Play();
		}

	}
}
