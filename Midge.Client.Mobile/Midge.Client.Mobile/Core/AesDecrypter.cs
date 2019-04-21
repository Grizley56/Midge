using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Midge.Client.Mobile.Core
{
	public class AesDecrypter : IDecrypter
	{
		private readonly ICryptoTransform _encrypter;
		private readonly ICryptoTransform _decrypter;

		private readonly SymmetricAlgorithm _algorithm;

		private static readonly Random _random = new Random();

		public static AesDecrypter Create()
		{
			byte[] password = new byte[128];
			_random.NextBytes(password);

			var rdb = new Rfc2898DeriveBytes(password, new byte[] {
				0x53,0x6f,0x64,0x69,0x75,0x6d,0x20,
				0x43,0x68,0x6c,0x6f,0x72,0x69,0x64,0x65
			}, 1);

			var key = rdb.GetBytes(32);
			var iv = rdb.GetBytes(16);

			return Create(key, iv);
		}

		public static AesDecrypter Create(byte[] key, byte[] iv)
		{
			var algorithm = Rijndael.Create();

			algorithm.Padding = PaddingMode.ISO10126;
			algorithm.Key = key;
			algorithm.IV = iv;

			return new AesDecrypter(algorithm);
		}

		private AesDecrypter(SymmetricAlgorithm alg)
		{
			_algorithm = alg;

			_encrypter = _algorithm.CreateEncryptor();
			_decrypter = _algorithm.CreateDecryptor();
		}


		public byte[] Encrypt(byte[] data)
		{
			using (var ms = new MemoryStream())
			{
				using (var cs = new CryptoStream(ms, _encrypter, CryptoStreamMode.Write))
				{
					cs.Write(data, 0, data.Length);
					cs.Close();
					return ms.ToArray();
				}
			}
		}

		public byte[] Decrypt(byte[] data)
		{
			using (var ms = new MemoryStream())
			using (var cs = new CryptoStream(ms, _decrypter, CryptoStreamMode.Write))
			{
				cs.Write(data, 0, data.Length);
				cs.Close();
				return ms.ToArray();
			}
		}

		public byte[] Key => _algorithm.Key;
		public byte[] IV => _algorithm.IV;
	}
}
