using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Midge.Server.Tcp
{
	public class TcpMessage: ITcpMessage
	{
		private readonly byte[] _data;
		public DateTime DateTime { get; }

		public static TcpMessage Empty { get; } = new TcpMessage(new byte[0]);

		public TcpMessage([NotNull] IEnumerable<byte> data)
		{
			_data = data.ToArray();
		}

		public TcpMessage(string message)
		{
			_data = Encoding.UTF8.GetBytes(message);
		}

		public byte[] GetMessage()
		{
			return _data;
		}
	}

	public interface ITcpMessage
	{
		byte[] GetMessage();
		DateTime DateTime { get; }
	}
}
