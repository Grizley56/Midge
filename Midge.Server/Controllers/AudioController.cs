using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Midge.Server.Communication;
using Midge.Server.Communication.Core;
using Midge.Server.Core;
using Midge.Server.Services;
using Newtonsoft.Json.Linq;

namespace Midge.Server.Controllers
{
	[MidgeController("audio")]
	public class AudioController: ControllerBase
	{
		private readonly IVolumeService _volumeService;
		private readonly IAudioStreamService _audioStreamService;

		public AudioController(MidgeContext context, IServiceManager serviceManager) 
			: base(context, serviceManager)
		{
			_volumeService = Services.GetService<IVolumeService>();
			_audioStreamService = Services.GetService<IAudioStreamService>();
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

		[UsedImplicitly]
		[MidgeCommand("startAudioStream")]
		public void StartAudioStream(
			[MidgeParameter("port", true)] int port,
			[MidgeParameter("sample_rate", false)] int? sampleRate = null,
			[MidgeParameter("bit_depth", false)] int? bitDepth = null,
			[MidgeParameter("channel", false)]  AudioChannel? channel = null)
		{
			if (sampleRate == null)
				sampleRate = 44100;

			if (bitDepth == null)
				bitDepth = 16;

			if (channel == null)
				channel = AudioChannel.Mono;

			var settings = new BroadcastSettings(sampleRate.Value, channel.Value, bitDepth.Value);


			IAudioSource audioSource = _audioStreamService.CreateSource(settings);
			
			IEncrypter encrypter = AesEncrypter.Create();

			BroadcastPoint point = new BroadcastPoint(audioSource, encrypter, Context.CurrentConnection.Ip, port);

			Context.AudioBroadcaster.Register(point);

			string base64Key = Convert.ToBase64String(encrypter.Key);
			string base64Iv = Convert.ToBase64String(encrypter.IV);

			Response = new JObject(
				new JProperty("crypt_key", base64Key),
				new JProperty("crypt_iv", base64Iv),
				new JProperty("sample_rate", settings.SampleRate),
				new JProperty("channel", (int)settings.Channel),
				new JProperty("alg", "aes32"));
		}

		[UsedImplicitly]
		[MidgeCommand("stopAudioStream")]
		public void StopAudioStream()
		{
			Context.AudioBroadcaster.UnRegister(Context.CurrentConnection.Ip);
		}

	}
}
