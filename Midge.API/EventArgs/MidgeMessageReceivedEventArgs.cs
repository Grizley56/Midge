using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.API.EventArgs
{
	public class MidgeMessageReceivedEventArgs: System.EventArgs
	{
		public readonly string Message;

		public MidgeMessageReceivedEventArgs(string message)
		{
			Message = message;
		}
	}
}
