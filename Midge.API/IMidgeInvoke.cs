using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.API
{
	public interface IMidgeInvoke
	{
		Task SendAsync(string methodName, MidgeParameters parameters);
		Task<T> SendAndWaitAsync<T>(string methodName, MidgeParameters parameters, int timeout = 100000);

		void Send(string method, MidgeParameters parameters);
		T SendAndWait<T>(string methodName, MidgeParameters parameters, int timeout = 100000);
	}
}
