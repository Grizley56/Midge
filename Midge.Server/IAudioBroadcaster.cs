using System.Net;

namespace Midge.Server
{
	public interface IAudioBroadcaster
	{
		void Register(BroadcastPoint broadcast);
		void UnRegister(IPAddress ip);

		void Start();
		void Stop();
	}
}