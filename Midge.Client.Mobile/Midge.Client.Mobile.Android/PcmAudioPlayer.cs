using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Midge.Client.Mobile.Droid;
using Xamarin.Forms;
using Encoding = Android.Media.Encoding;

[assembly:Dependency(typeof(PcmAudioPlayer))]
namespace Midge.Client.Mobile.Droid
{
	public class PcmAudioPlayer: IPcmAudioPlayer
	{
		private bool _isPlaying;

		private AudioTrack _audioTrack;

		public PcmAudioPlayer()
		{
			
		}

		public void AddRawData(byte[] pcm)
		{
			if (!_isPlaying)
				return;

			_audioTrack?.Write(pcm, 0, pcm.Length);
		}

		public async Task AddRawDataAsync(byte[] pcm)
		{
			if (!_isPlaying)
				return;

			await _audioTrack.WriteAsync(pcm, 0, pcm.Length);
		}


		public void Stop()
		{
			if (!_isPlaying)
				return;

			_audioTrack.Stop();
			_audioTrack = null;

			_isPlaying = false;
		}

		public bool IsPlaying => _isPlaying;

		public void Start(int sampleRate, int bitDepth, bool stereo, int buffSize)
		{
			if (_isPlaying)
				return;

			Encoding encoding;

			switch (bitDepth)
			{
				case 8:
					encoding = Encoding.Pcm8bit;
					break;
				case 16:
					encoding = Encoding.Pcm16bit;
					break;
				case 32:
					encoding = Encoding.PcmFloat;
					break;
				default:
					throw new NotImplementedException();
			}

			_isPlaying = true;

#pragma warning disable 618
			_audioTrack = new AudioTrack(Stream.Music, sampleRate,
				stereo ? ChannelOut.Stereo : ChannelOut.Mono, encoding, buffSize,
				AudioTrackMode.Stream);
#pragma warning restore 618

			_audioTrack.Play();
		}
	}
}