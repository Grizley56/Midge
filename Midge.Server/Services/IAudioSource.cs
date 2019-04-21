using System;
using System.Collections.Generic;
using System.Text;

namespace Midge.Server.Services
{
	public interface IAudioSource
	{
		bool TryDequeue(out byte[] data);
	}
}
