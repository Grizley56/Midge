using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.API.Core
{
	interface IWaiter<T>
	{
		void SetReady(T value);
		T Result { get; }
	}
}
