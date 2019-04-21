using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using Midge.Client.Mobile.Model;

namespace Midge.Client.Mobile.Core
{
	public class AudioStreamClient: INotifyPropertyChanged
	{
		private IPcmAudioPlayer _player;
		private IAudioReceiver _receiver;

		public bool IsStarted
		{
			get => _isStarted;
			private set
			{
				if (value == _isStarted) return;
				_isStarted = value;
				OnPropertyChanged();
			}
		}

		private readonly object _lock = new object();
		private bool _isStarted;

		public AudioStreamClient(IPcmAudioPlayer player, IAudioReceiver receiver)
		{
			_player = player;
			_receiver = receiver;
		}

		public void Start(AudioStreamModel streamModel)
		{
			lock (_lock)
			{
				if (IsStarted)
					return;

				_receiver.AudioReceived += AudioReceived;
				_receiver.StartReceive();
				_player.Start(streamModel.SampleRate, streamModel.BitDepth, streamModel.Channel == API.Models.AudioChannel.Stereo,
					4096);

				IsStarted = true;
			}
		}

		private void AudioReceived(object sender, AudioReceivedEventArgs e)
		{
			_player.AddRawData(e.Data);
		}

		public void Stop()
		{
			lock (_lock)
			{
				if (!IsStarted)
					return;

				_receiver.StopReceive();
				_receiver.AudioReceived -= AudioReceived;
				_player.Stop();

				IsStarted = false;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
