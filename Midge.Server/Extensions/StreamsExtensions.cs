using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Midge.Server.Extensions
{
	public static class StreamsExtensions
	{
		public static Task WriteAsync(this Stream stream, byte[] data, CancellationToken token = default(CancellationToken))
		{
			return stream.WriteAsync(data, 0, data.Length, token);
		}

		public static void Write(this Stream stream, byte[] data)
		{
			stream.Write(data, 0, data.Length);
		}
	}
}
