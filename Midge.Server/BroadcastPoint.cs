using System.Net;
using Midge.Server.Core;
using Midge.Server.Services;

namespace Midge.Server
{
	public class BroadcastPoint
	{
		public IAudioSource AudioSource { get; }
		public IPAddress Ip { get; }
		public int Port { get; }
		public IEncrypter Encrypter { get; }

		public BroadcastPoint(IAudioSource audioSource, IEncrypter encrypter, IPAddress ip, int port)
		{
			AudioSource = audioSource;
			Encrypter = encrypter;
			Ip = ip;
			Port = port;
		}
	}
}