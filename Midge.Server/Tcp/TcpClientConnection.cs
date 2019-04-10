using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Midge.Server.Extensions;

namespace Midge.Server.Tcp
{
	public class TcpClientConnection: ITcpClientConnection
	{
		private readonly SemaphoreSlim _semaphore;

		protected TcpClient TcpClient { get; }
		protected ConcurrentQueue<ITcpMessage> MessageQueue;

		protected NetworkStream NetworkStream { get; }
		protected Stream ReadWriteStream { get; set; }
		
		public IReadOnlyCollection<ITcpMessage> MessagesInQueue => MessageQueue;
		public IPAddress Ip { get; }
		public Guid ConnectionId { get; }
		public TimeSpan ReceiveTimeout { get; set; }

		public void Disconnect()
		{
			if (IsConnected())
				TcpClient.Close();
		}

		public bool IsConnected()
		{
			return TcpClient.Client.IsConnected();
		}

		public TcpClientConnection(TcpClient tcpClient)
		{
			_semaphore = new SemaphoreSlim(1);

			TcpClient = tcpClient;

			MessageQueue = new ConcurrentQueue<ITcpMessage>();

			NetworkStream = TcpClient.GetStream();
			ReadWriteStream = NetworkStream;
			
			Ip = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address;
			ConnectionId = Guid.NewGuid();
		}

		public virtual async Task SendMessageAsync(ITcpMessage message, CancellationToken token)
		{
			MessageQueue.Enqueue(message);

			await _semaphore.WaitAsync(token).ConfigureAwait(false);
			
			try
			{
				if (NetworkStream.CanWrite)
				{
					var byteArray = message.GetMessage();

					var messageLen = BitConverter.GetBytes(byteArray.Length);
					
					await ReadWriteStream.WriteAsync(messageLen, token)
						.ConfigureAwait(false);

					await ReadWriteStream.FlushAsync(token)
						.ConfigureAwait(false);

					await ReadWriteStream.WriteAsync(byteArray, token)
						.ConfigureAwait(false);

					await ReadWriteStream.FlushAsync(token)
						.ConfigureAwait(false);
				}
			}
			finally
			{
				MessageQueue.TryDequeue(out var massage);
				_semaphore.Release();
			}
		}

		public virtual void SendMessage(ITcpMessage message)
		{
			MessageQueue.Enqueue(message);

			_semaphore.Wait();

			try
			{
				if (NetworkStream.CanWrite)
				{
					var byteArray = message.GetMessage();

					var messageLen = BitConverter.GetBytes(byteArray.Length);
					ReadWriteStream.Write(messageLen);
					ReadWriteStream.Flush();

					ReadWriteStream.Write(byteArray);
					ReadWriteStream.Flush();
				}
			}
			finally
			{
				MessageQueue.TryDequeue(out var massage);
				_semaphore.Release();
			}
		}

		public ITcpMessage ReadMessage()
		{
			if (!IsConnected())
				throw new Exception("Connection closed");

			byte[] headerBytes = new byte[4];
			ReadWriteStream.Read(headerBytes, 0, headerBytes.Length);
			int contentLen = BitConverter.ToInt32(headerBytes, 0);

			int bufferSize = MaxBufferSize;
			int totalRead = 0;

			TimeSpan timeout = TimeSpan.Zero;

			if (contentLen == 0)
				return TcpMessage.Empty;

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

					int readCount = ReadWriteStream.Read(buff, 0, bufferSize);
					totalRead += readCount;

					if (readCount > 0)
					{
						memory.Write(buff, 0, readCount);
						timeout = TimeSpan.Zero;
					}
					else
					{
						Thread.Sleep(ReadFrequency);
						timeout += TimeSpan.FromMilliseconds(ReadFrequency);
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

			return new TcpMessage(memory.ToArray());
		}

		private const int MaxBufferSize = 4096;
		private const int ReadFrequency = 100;

		public async Task<ITcpMessage> ReadMessageAsync(CancellationToken token)
		{
			if (!IsConnected())
				throw new Exception("Connection closed");

			byte[] headerBytes = new byte[4];
			await ReadWriteStream.ReadAsync(headerBytes, 0, headerBytes.Length, CancellationToken.None);
			int contentLen = BitConverter.ToInt32(headerBytes, 0);

			int bufferSize = MaxBufferSize;
			int totalRead = 0;

			TimeSpan timeout = TimeSpan.Zero;

			if (contentLen == 0)
				return TcpMessage.Empty;

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

					int readCount = ReadWriteStream.Read(buff, 0, bufferSize);
					totalRead += readCount;

					if (readCount > 0)
					{
						memory.Write(buff, 0, readCount);
						timeout = TimeSpan.Zero;
					}
					else
					{
						await Task.Delay(ReadFrequency, CancellationToken.None);
						timeout += TimeSpan.FromMilliseconds(ReadFrequency);
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

			return new TcpMessage(memory.ToArray());
		}

		public void Dispose()
		{
			TcpClient?.Dispose();
			_semaphore?.Dispose();
		}

		public override string ToString()
		{
			return $"{Ip} - {ConnectionId}";
		}

		protected bool Equals(TcpClientConnection other)
		{
			return Equals(Ip, other.Ip) && ConnectionId.Equals(other.ConnectionId);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((TcpClientConnection)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((Ip != null ? Ip.GetHashCode() : 0) * 397) ^ ConnectionId.GetHashCode();
			}
		}
	}
}
