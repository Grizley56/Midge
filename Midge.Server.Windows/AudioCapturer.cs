using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.SoundIn;

namespace Midge.Server.Windows
{
	public static class AudioCapturer
	{
		public static WasapiLoopbackCapture WasapiCapture { get; private set; }

		private static bool _isInited;

		public static void Init()
		{
			if (_isInited)
				return;
			
			WasapiCapture = new CSCore.SoundIn.WasapiLoopbackCapture(0);
			WasapiCapture.Initialize();
			WasapiCapture.Start();

			_isInited = true;
		}
	}
}
