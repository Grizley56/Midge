using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Midge.Client.Mobile.Core
{
	public class UdpAudioReceiver: IAudioReceiver
	{
		public event EventHandler<AudioReceivedEventArgs> AudioReceived;
		public int Port { get; }
		
		private UdpClient _udpClient;
		private CancellationTokenSource _tokenSource;
		
		public IDecrypter DecryptService { get; set; }

		public UdpAudioReceiver(int port)
		{
			Port = port;
		}

		public void StartReceive()
		{
			_tokenSource = new CancellationTokenSource();
			_udpClient = new UdpClient(Port);

			var address = Settings.ServerStreamAddress;
			if (address == null)
				throw new InvalidOperationException("Invalid ip address");

			_udpClient.Connect(address);

			Task.Run(() => Receive(_tokenSource.Token));
		}

		private void Receive(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				IPEndPoint ip = new IPEndPoint(IPAddress.Any, 0);
				byte[] data;

				try
				{
					data = _udpClient.Receive(ref ip);
				}
				catch (ObjectDisposedException)
				{
					break;
				}
				catch (SocketException ex) when (ex.ErrorCode == 10004)
				{
					break;
				}

				if (DecryptService != null)
					data = DecryptService.Decrypt(data);

				OnAudioReceived(new AudioReceivedEventArgs(data));
			}
		}


		public void StopReceive()
		{
			_tokenSource?.Cancel();
			_tokenSource?.Dispose();
			_udpClient?.Dispose();
		}

		protected virtual void OnAudioReceived(AudioReceivedEventArgs e)
		{
			AudioReceived?.Invoke(this, e);
		}
	}
}
