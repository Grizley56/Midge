using System;
using System.Collections.Generic;
using System.Text;

namespace Midge.Server.Core
{
	public interface IEncrypter
	{
		byte[] Encrypt(byte[] data);
		byte[] Decrypt(byte[] data);

		byte[] Key { get; }
		byte[] IV { get; }
	}
}
