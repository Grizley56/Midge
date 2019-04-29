using System;
using System.Collections.Generic;
using System.Text;

namespace Midge.Server.Services
{
	public interface ISystemService
	{
		void TurnOff(int delaySeconds);
		void Restart(int delaySeconds);
	}
}
