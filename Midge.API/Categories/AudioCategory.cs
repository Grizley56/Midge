using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midge.API.Models;

namespace Midge.API.Categories
{
	public class AudioCategory: Category
	{
		public async Task SetVolumeAsync(int value)
		{
			await MidgeInvoke.SendAsync("audio.setVolume", new MidgeParameters
			{
				{"value", value }
			});
		}

		public async Task<int> GetVolumeAsync()
		{
			return await MidgeInvoke.SendAndWaitAsync<int>("audio.getVolume", MidgeParameters.Empty, Timeout);
		}


		public async Task<bool> IsMutedAsync()
		{
			return await MidgeInvoke.SendAndWaitAsync<bool>("audio.getMute", MidgeParameters.Empty, Timeout);
		}

		public async Task SetMuteAsync(bool value)
		{
			await MidgeInvoke.SendAsync("audio.setMute", new MidgeParameters
			{
				{"value", value}
			});
		}

		public void SetVolume(int value)
		{
			MidgeInvoke.Send("audio.setVolume", new MidgeParameters
			{
				{"value", value }
			});
		}

		public int GetVolume()
		{
			return MidgeInvoke.SendAndWait<int>("audio.getVolume", MidgeParameters.Empty, Timeout);
		}


		public bool IsMuted()
		{
			return MidgeInvoke.SendAndWait<bool>("audio.getMute", MidgeParameters.Empty, Timeout);
		}

		public void SetMute(bool value)
		{
			MidgeInvoke.Send("audio.setMute", new MidgeParameters
			{
				{"value", value}
			});
		}


		public AudioStreamInfo StartAudioStream(int port, int sampleRate = 44100, int bitDepth = 128, AudioChannel channel = AudioChannel.Mono)
		{
			return MidgeInvoke.SendAndWait<AudioStreamInfo>("audio.startAudioStream", new MidgeParameters
			{
				{"port", port},
				{"sample_rate", sampleRate},
				{"bit_depth", bitDepth},
				{"channel", (int)channel}
			});
		}

		public Task<AudioStreamInfo> StartAudioStreamAsync(int port, int sampleRate = 44100, int bitDepth = 128, AudioChannel channel = AudioChannel.Mono)
		{
			return MidgeInvoke.SendAndWaitAsync<AudioStreamInfo>("audio.startAudioStream", new MidgeParameters
			{
				{"port", port},
				{"sample_rate", sampleRate},
				{"bit_depth", bitDepth},
				{"channel", (int)channel}
			});
		}

		public void StopAudioStream()
		{
			MidgeInvoke.Send("audio.stopAudioStream", MidgeParameters.Empty);
		}

		public Task StopAudioStreamAsync()
		{
			return MidgeInvoke.SendAsync("audio.stopAudioStream", MidgeParameters.Empty);
		}

		public AudioCategory(IMidgeInvoke midgeInvoke) 
			: base(midgeInvoke)
		{

		}
	}
}
