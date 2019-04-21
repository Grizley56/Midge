using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Midge.API.Models;

namespace Midge.Client.Mobile
{
	public interface IPcmAudioPlayer
	{
		void AddRawData(byte[] pcm);
		Task AddRawDataAsync(byte[] pcm);
		void Start(int sampleRate, int bitDepth, bool stereo, int buffSize);
		void Stop();

		bool IsPlaying { get; }
	}
}
