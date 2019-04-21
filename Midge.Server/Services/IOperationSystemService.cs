using System;
using System.Collections.Generic;
using System.Text;

namespace Midge.Server.Services
{
	public interface IOperationSystemService
	{
		void TurnOff(int delay = 0);
		void Restart(int delay = 0);
	}
}
