using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Midge.API.EventArgs;

namespace Midge.API
{
	public class MidgeRestClient
	{
		public Encoding Encoding { get; set; }
		public TcpClient Tcp { get; private set; }

		public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromSeconds(3);

		public TimeSpan UpdateFrequency { get; set; }
		public TimeSpan ReceiveTimeout { get; } = TimeSpan.FromSeconds(5);

		public event EventHandler<System.EventArgs> Started;
		public event EventHandler<System.EventArgs> Stopped;
		public event EventHandler<System.EventArgs> Faulted;


		private SslStream _sslStream;
		private NetworkStream _networkStream;

		public event EventHandler<MidgeMessageReceivedEventArgs> ServerMessageReceived;

		private SemaphoreSlim _semaphore;
		private ConfiguredTaskAwaitable _internalTask;
		private CancellationTokenSource _tokenSource;

		public bool IsStarted { get; private set; }

		public MidgeRestClient(Encoding encoding)
		{
			Encoding = encoding;
			UpdateFrequency = TimeSpan.FromMilliseconds(100);
		}

		public MidgeRestClient()
			:this(Encoding.UTF8)
		{
			
		}

		private void Connect(IPEndPoint ip)
		{
			Tcp = new TcpClient();

			var result = Tcp.BeginConnect(ip.Address, ip.Port, null, null);

			var success = result.AsyncWaitHandle.WaitOne(ConnectionTimeout);

			if (!success)
			{
				throw new Exception("Failed to connect.");
			}

			Tcp.EndConnect(result);

			_networkStream = Tcp.GetStream();

			_sslStream = new SslStream(_networkStream, true, 
				ValidateServerCertificate,
				null);
			_sslStream.AuthenticateAsClient("Midge");
		}

		private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
		{
			return true;
		}

		public async Task Start(IPEndPoint ip)
		{
			if (IsStarted)
				return;

			_semaphore = new SemaphoreSlim(1);
			_tokenSource = new CancellationTokenSource();

			Connect(ip);
			_internalTask = StartReceive(_tokenSource.Token).ConfigureAwait(false);
			IsStarted = true;
			OnStarted();
		}

		public async Task Stop()
		{
			if (!IsStarted)
				return;

			_tokenSource.Cancel();
			await _internalTask;
			IsStarted = false;
			OnStopped();
		}

		
		private async Task StartReceive(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				if (!_networkStream.DataAvailable || !_networkStream.CanRead)
				{
					await Task.Delay(UpdateFrequency, token);
					continue;
				}
				byte[] data;

				try
				{
					data = await ReadAvailableData(token);
				}
				catch
				{
					IsStarted = false;
					OnFaulted();
					return;
				}


				if (data == null)
					break;

				ServerMessageReceived?.Invoke(this, new MidgeMessageReceivedEventArgs(Encoding.GetString(data)));
			}

			Tcp?.Close();
			IsStarted = false;
			OnStopped();
		}

		public async Task SendMessageAsync(byte[] message)
		{
			await _semaphore.WaitAsync();

			try
			{
				await _sslStream.WriteAsync(BitConverter.GetBytes(message.Length), 0, sizeof(int));
				await _sslStream.WriteAsync(message, 0, message.Length);
				await _sslStream.FlushAsync();
			}
			finally
			{
				_semaphore.Release();
			}
		}

		public Task SendMessageAsync(string message)
		{
			return SendMessageAsync(Encoding.GetBytes(message));
		}

		public void SendMessage(byte[] message)
		{
			_semaphore.Wait();

			try
			{
				_sslStream.Write(BitConverter.GetBytes(message.Length), 0, sizeof(int));
				_sslStream.Write(message, 0, message.Length);
				_sslStream.Flush();
			}
			finally
			{
				_semaphore.Release();
			}
		}

		public void SendMessage(string message)
		{
			SendMessage(Encoding.GetBytes(message));
		}


		protected async Task<byte[]> ReadAvailableData(CancellationToken token)
		{
			byte[] headerBytes = new byte[4];
			
			await _sslStream.ReadAsync(headerBytes, 0, headerBytes.Length, token);

			int contentLen = BitConverter.ToInt32(headerBytes, 0);

			int bufferSize = 4096;
			int totalRead = 0;
			TimeSpan timeout = TimeSpan.Zero;

			if (contentLen == 0)
				return new byte[0];

			MemoryStream memory = new MemoryStream();
			byte[] buff = new byte[bufferSize];

			try
			{
				while (totalRead < contentLen)
				{
					if (timeout > ReceiveTimeout)
						throw new TimeoutException();

					if (contentLen - totalRead < bufferSize)
						bufferSize = contentLen - totalRead;

					int readcount = _sslStream.Read(buff, 0, bufferSize);
					totalRead += readcount;

					if (readcount > 0)
					{
						memory.Write(buff, 0, readcount);
						timeout = TimeSpan.Zero;
					}
					else
					{
						await Task.Delay(UpdateFrequency, token);
						timeout += UpdateFrequency;
					}
				}
			}
			catch (Exception ex)
			{
				if (!(ex is IOException) && !(ex is TimeoutException))
					throw;

				return null;
			}

			memory.Close();
			return memory.ToArray();
		}

		protected virtual void OnFaulted()
		{
			Faulted?.Invoke(this, System.EventArgs.Empty);
		}

		protected virtual void OnStarted()
		{
			Started?.Invoke(this, System.EventArgs.Empty);
		}

		protected virtual void OnStopped()
		{
			Stopped?.Invoke(this, System.EventArgs.Empty);
		}
	}
}
