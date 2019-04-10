using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Midge.Client.Mobile;
using Midge.Client.Mobile.Core;
using Midge.Client.Mobile.Model;
//using MidgeContract;
using Xamarin.Forms;

namespace Midge.Client.Mobile.ViewModel
{
	public class SoundViewModel
	{
		public SoundModel Sound { get; set; } = new SoundModel();

		private int _previousVolumeValue;
		private bool _previousMuteValue;

		public SoundViewModel()
		{
			MidgeCore.Instance.ConnectionStateChanged += ConnectionStateChanged;

			Device.StartTimer(TimeSpan.FromMilliseconds(500), UpdateSound);
		}

		private async void ConnectionStateChanged(object sender, EventArgs e)
		{
			ConnectionState state = MidgeCore.Instance.State;
			if (state == ConnectionState.Connected)
			{
				Sound.Volume = await MidgeCore.Instance.Client.Audio.GetVolumeAsync();
				Sound.IsMuted = await MidgeCore.Instance.Client.Audio.IsMutedAsync();
			}
		}

		private bool UpdateSound()
		{
			if (MidgeCore.Instance.State != ConnectionState.Connected)
				return true;

			UpdateVolume();
			UpdateMute();

			return true;
		}

		private void UpdateVolume()
		{
			var volume = Sound.Volume;

			if (_previousVolumeValue == volume)
				return;

			MidgeCore.Instance.Client.Audio.SetVolume(volume);
			_previousVolumeValue = volume;
		}

		private void UpdateMute()
		{
			var mute = Sound.IsMuted;

			if (_previousMuteValue == mute)
				return;

			MidgeCore.Instance.Client.Audio.SetMute(mute);
			_previousMuteValue = mute;
		}
	}
}
