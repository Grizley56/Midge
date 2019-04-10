using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public AudioCategory(IMidgeInvoke midgeInvoke) 
			: base(midgeInvoke)
		{

		}
	}
}
