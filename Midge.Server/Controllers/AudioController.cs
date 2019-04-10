using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Midge.Server.Core;
using Midge.Server.Services;
using Newtonsoft.Json.Linq;

namespace Midge.Server.Controllers
{
	[MidgeController("audio")]
	public class AudioController: ControllerBase
	{
		private readonly IVolumeService _volumeService;

		public AudioController(IContext context, IServiceManager serviceManager) 
			: base(context, serviceManager)
		{
			_volumeService = Services.GetService<IVolumeService>();
		}

		[UsedImplicitly]
		[MidgeCommand("setVolume")]
		public void SetVolume([MidgeParameter("value")] int value)
		{
			_volumeService.Volume = value;
		}

		[UsedImplicitly]
		[MidgeCommand("getVolume")]
		public void GetVolume()
		{
			Response = new JValue(_volumeService.Volume);
		}

		[UsedImplicitly]
		[MidgeCommand("setMute")]
		public void SetMute([MidgeParameter("value")] bool value)
		{
			_volumeService.IsMute = value;
		}

		[UsedImplicitly]
		[MidgeCommand("getMute")]
		public void GetMute()
		{
			Response = new JValue(_volumeService.IsMute);
		}

	}
}
