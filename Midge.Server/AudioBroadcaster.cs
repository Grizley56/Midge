using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Midge.Server.Core;
using Midge.Server.Web.Udp;

namespace Midge.Server
{
	public class AudioBroadcaster: IAudioBroadcaster
	{
		private readonly IUdpServer _server;
		private CancellationTokenSource _token;

		private readonly ConcurrentDictionary<IPAddress, BroadcastPoint> _broadcasts;

		public AudioBroadcaster(IUdpServer server)
		{
			_server = server;
			_broadcasts = new ConcurrentDictionary<IPAddress, BroadcastPoint>();
		}

		public void Register(BroadcastPoint broadcastPoint)
		{
			_broadcasts.TryAdd(broadcastPoint.Ip, broadcastPoint);
		}

		public void UnRegister(IPAddress ip)
		{
			_broadcasts.TryRemove(ip, out var broadcastPoint);
			if (broadcastPoint.AudioSource is IDisposable disposable)
				disposable?.Dispose();
		}

		public void Start()
		{
			_token = new CancellationTokenSource();
			Task.Run(() => SendAudioData(_token.Token));
		}

		private void SendAudioData(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				var snapshot = _broadcasts.Values;
				foreach (var point in snapshot)
				{
					while (point.AudioSource.TryDequeue(out var data))
					{
						byte[] encryptData = point.Encrypter.Encrypt(data);
						_server.Send(new IPEndPoint(point.Ip, point.Port), encryptData);
					}
				}
			}
		}

		public void Stop()
		{
			_token.Cancel();
		}
	}
}
