using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Midge.API.Core
{
	public class ResponseWaiter: IWaitable, IWaiter<JToken>
	{
		private bool _isReady;
		private readonly object _isReadyLock = new object();

		public async Task WaitAsync(int timeout)
		{
			using (CancellationTokenSource token = new CancellationTokenSource(timeout))
			{
				while (!IsReady)
				{
					try
					{
						await Task.Delay(UpdateFrequency, token.Token);
					}
					catch (OperationCanceledException)
					{
						throw new TimeoutException();
					}
				}
			}
		}

		public void Wait(int timeout)
		{
			using (CancellationTokenSource token = new CancellationTokenSource(timeout))
			{
				while (!IsReady)
				{
					if (token.IsCancellationRequested)
						throw new TimeoutException();

					Thread.Sleep(UpdateFrequency);
				}
			}
		}

		private TimeSpan UpdateFrequency { get; } = TimeSpan.FromMilliseconds(50);

		public bool IsReady
		{
			get
			{
				lock (_isReadyLock)
					return _isReady;
			}
			private set
			{
				lock (_isReadyLock)
					_isReady = value;
			}
		}

		public Guid Key { get; }

		public ResponseWaiter(Guid key)
		{
			Key = key;
		}

		
		public void SetReady(JToken value)
		{
			if (IsReady)
				throw new Exception();

			Result = value;
			IsReady = true;
		}

		public JToken Result { get; private set; }
	}
}
