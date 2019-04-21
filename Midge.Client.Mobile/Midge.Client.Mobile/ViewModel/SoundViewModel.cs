using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using Midge.Client.Mobile;
using Midge.Client.Mobile.Core;
using Midge.Client.Mobile.Model;
using Xamarin.Forms;

namespace Midge.Client.Mobile.ViewModel
{
	public class SoundViewModel: INotifyPropertyChanged
	{
		public SoundModel Sound { get; set; } = new SoundModel();
		public AudioStreamModel AudioStream { get; set; } = new AudioStreamModel();
		public AudioStreamClient AudioStreamClient { get; private set; }

		public int[] SampleRates { get; } = new int[4] {8000, 11025, 22050, 44100};
		public int[] BitDepths { get; } = new int[3] {8, 16, 32};

		private int _previousVolumeValue;
		private bool _previousMuteValue;

		public ICommand ToggleAudioStream { get; private set; }

		private UdpAudioReceiver _udpAudioReceiver;
		private IPcmAudioPlayer _player;
		private bool _audioStreamToggled;

		public bool AudioStreamToggled
		{
			get => _audioStreamToggled;
			set
			{
				if (value == _audioStreamToggled) return;
				_audioStreamToggled = value;
				OnPropertyChanged();
			}
		}

		public SoundViewModel()
		{
			MidgeCore.Instance.ConnectionStateChanged += ConnectionStateChanged;

			Device.StartTimer(TimeSpan.FromMilliseconds(500), UpdateSound);

			_udpAudioReceiver = new UdpAudioReceiver(Settings.StreamingClientPort);
			_player = DependencyService.Get<IPcmAudioPlayer>();

			AudioStreamClient = new AudioStreamClient(_player, _udpAudioReceiver);

			ToggleAudioStream = new Command(ToggleStartStopStream);
		}

		private async void ToggleStartStopStream()
		{
			AudioStreamToggled = true;

			await Task.Delay(1000);

			try
			{
				if (AudioStreamClient.IsStarted)
				{
					await Task.Run(StopStream);
				}
				else
				{
					await Task.Run(StartStream);
				}
			}
			finally
			{
				AudioStreamToggled = false;
			}
		}

		private void StartStream()
		{
			var settings = MidgeCore.Instance.Client.Audio.StartAudioStream(_udpAudioReceiver.Port, AudioStream.SampleRate, AudioStream.BitDepth,
				AudioStream.Channel);

			AudioStream.SampleRate = settings.SampleRate;
			AudioStream.Channel = settings.Channel;

			AesDecrypter aes =
				AesDecrypter.Create(Convert.FromBase64String(settings.Key), Convert.FromBase64String(settings.IV));

			_udpAudioReceiver.DecryptService = aes;

			AudioStreamClient.Start(AudioStream);
		}

		private void StopStream()
		{
			AudioStreamClient.Stop();
			MidgeCore.Instance.Client.Audio.StopAudioStream();
			_udpAudioReceiver.DecryptService = null;
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


		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		}
	}
}
