using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Midge.Core;
using Midge.Model;
using Midge;
using MidgeContract;
using Xamarin.Forms;

namespace Midge.ViewModel
{
	public class SoundViewModel
	{
		public SoundModel Sound { get; set; } = new SoundModel();

		private int _previousVolumeValue;
		private bool _previousMuteValue;

		public SoundViewModel()
		{
			ReconnectableChannel<IMidge>.MidgeInstance.ChannelStateChanged += MidgeInstance_ChannelStateChanged;
			
			Device.StartTimer(TimeSpan.FromMilliseconds(500), UpdateSound);
		}

		private void MidgeInstance_ChannelStateChanged(object sender, EventArgs e)
		{
			if (ReconnectableChannel<IMidge>.MidgeInstance.State == System.ServiceModel.CommunicationState.Opened)
			{
				ReconnectableChannel<IMidge>.MidgeInstance.Invoke(i => Sound.Volume = i.GetVolume());
				ReconnectableChannel<IMidge>.MidgeInstance.Invoke(i => Sound.IsMuted = i.IsMute());
			}
		}

		private bool UpdateSound()
		{
			UpdateVolume();
			UpdateMute();
			return true;
		}

		private void UpdateVolume()
		{
			var volume = Sound.Volume;

			if (_previousVolumeValue == volume)
				return;

			if (ReconnectableChannel<IMidge>.MidgeInstance.Invoke(i => i.SetVolume(volume)))
				_previousVolumeValue = volume;
		}

		private void UpdateMute()
		{
			var mute = Sound.IsMuted;

			if (_previousMuteValue == mute)
				return;

			if (ReconnectableChannel<IMidge>.MidgeInstance.Invoke(i => i.SetMute(mute)))
				_previousMuteValue = mute;
		}
	}
}
