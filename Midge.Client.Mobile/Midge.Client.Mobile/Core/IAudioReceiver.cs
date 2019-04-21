using System;
using System.Collections.Generic;
using System.Text;

namespace Midge.Client.Mobile.Core
{
	public interface IAudioReceiver
	{
		event EventHandler<AudioReceivedEventArgs> AudioReceived;
		void StartReceive();
		void StopReceive();
	}

	public class AudioReceivedEventArgs: EventArgs
	{
		public readonly byte[] Data;

		public AudioReceivedEventArgs(byte[] data)
		{
			Data = data;
		}
	}
}
