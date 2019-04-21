using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.SoundIn;
using CSCore.Streams;
using Midge.Server.Services;

namespace Midge.Server.Windows.Services
{
	public class AudioStreamService : IAudioStreamService
	{
		public Server.Services.IAudioSource CreateSource(BroadcastSettings settings)
		{
			SoundInSourceWrapper source = new SoundInSourceWrapper(
				new SoundInSource(AudioCapturer.WasapiCapture) {FillWithZeros = false}, settings);

			return source;
		}
	}
}
