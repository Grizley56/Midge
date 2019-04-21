using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;
using Midge.Server.Services;

namespace Midge.Server.Windows.Services
{
	public class VolumeService: IVolumeService
	{
		private readonly CoreAudioController _audioController;

		public VolumeService()
		{
			_audioController = new CoreAudioController();
		}

		public int Volume
		{
			get => (int)_audioController.DefaultPlaybackDevice.Volume;
			set
			{
				if (value < 0 || value > 100)
					throw new ArgumentOutOfRangeException(nameof(value));

				_audioController.DefaultPlaybackDevice.SetVolumeAsync(value).ConfigureAwait(false).GetAwaiter().GetResult();
			}
		}

		public bool IsMute
		{
			get => _audioController.DefaultPlaybackDevice.IsMuted;
			set => _audioController.DefaultPlaybackDevice.MuteAsync(value).ConfigureAwait(false).GetAwaiter().GetResult();
		}
	}
}
