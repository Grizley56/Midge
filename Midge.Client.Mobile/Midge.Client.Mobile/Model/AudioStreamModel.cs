using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Midge.API.Models;

namespace Midge.Client.Mobile.Model
{
	public class AudioStreamModel: INotifyPropertyChanged
	{
		private int _sampleRate;
		private int _bitDepth;
		private AudioChannel _channel;
		public event PropertyChangedEventHandler PropertyChanged;

		public AudioStreamModel()
		{
			SampleRate = 44100;
			BitDepth = 16;
			Channel = AudioChannel.Mono;
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public int SampleRate
		{
			get => _sampleRate;
			set
			{
				if (value == _sampleRate) return;
				_sampleRate = value;
				OnPropertyChanged();
			}
		}

		public int BitDepth
		{
			get => _bitDepth;
			set
			{
				if (value == _bitDepth) return;
				_bitDepth = value;
				OnPropertyChanged();
			}
		}

		public AudioChannel Channel
		{
			get => _channel;
			set
			{
				if (value == _channel) return;
				_channel = value;
				OnPropertyChanged();
			}
		}
	}
}
