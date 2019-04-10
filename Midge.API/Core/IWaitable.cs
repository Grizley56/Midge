using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.API.Core
{
	interface IWaitable
	{
		Task WaitAsync(int timeout);
		void Wait(int timeout);
		bool IsReady { get; }
	}
}
