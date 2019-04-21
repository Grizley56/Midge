using System;
using System.Collections.Generic;
using System.Text;

namespace Midge.Server.Services
{
	public interface IAudioStreamService
	{
		IAudioSource CreateSource(BroadcastSettings settings);
	}
}
