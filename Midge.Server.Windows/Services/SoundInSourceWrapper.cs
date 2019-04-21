using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.SoundIn;
using CSCore.Streams;
using Midge.Server.Services;

namespace Midge.Server.Windows.Services
{
	public class SoundInSourceWrapper: IDisposable, Server.Services.IAudioSource
	{
		private readonly SoundInSource _soundIn;
		private readonly IWaveSource _convertedSource;

		public BroadcastSettings Settings { get; private set; }

		private ConcurrentQueue<byte[]> _audioChunks;

		public SoundInSourceWrapper(SoundInSource soundIn, BroadcastSettings settings)
		{
			Settings = settings;

			_soundIn = soundIn;

			_convertedSource =
				soundIn.ChangeSampleRate(Settings.SampleRate).ToSampleSource().ToWaveSource(Settings.BitDepth);

			if (settings.Channel == AudioChannel.Mono)
				_convertedSource = _convertedSource.ToMono();
			else
				_convertedSource = _convertedSource.ToStereo();

			_audioChunks = new ConcurrentQueue<byte[]>();

			_soundIn.DataAvailable += SoundInDataAvailable;
		}
		
		private void SoundInDataAvailable(object sender, DataAvailableEventArgs e)
		{
			byte[] buffer = new byte[_convertedSource.WaveFormat.BytesPerSecond / 2];
			int read;

			while ((read = _convertedSource.Read(buffer, 0, buffer.Length)) > 0)
			{
				var dataToSend = new List<byte>();
				dataToSend.AddRange(buffer.Take(read).ToArray());
				var data = dataToSend.ToArray();
				_audioChunks.Enqueue(data);
			}
		}

		public void Dispose()
		{
			_soundIn.DataAvailable -= SoundInDataAvailable;
			_audioChunks = null;
			_soundIn?.Dispose();
		}


		public bool TryDequeue(out byte[] data)
		{
			if (_audioChunks == null)
			{
				data = new byte[0];
				return false;
			}

			return _audioChunks.TryDequeue(out data);
		}
	}
}
