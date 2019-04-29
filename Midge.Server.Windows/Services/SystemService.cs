using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using Midge.Server.Services;

namespace Midge.Server.Windows.Services
{
	public class SystemService: ISystemService
	{
		public void TurnOff(int delaySeconds)
		{
			if (delaySeconds < 0)
				throw new ArgumentException(nameof(delaySeconds));

			CmdExecuteAndForget($"restart -s -t {delaySeconds}");
		}

		public void Restart(int delaySeconds)
		{
			if (delaySeconds < 0)
				throw new ArgumentException(nameof(delaySeconds));

			CmdExecuteAndForget($"restart -r -t {delaySeconds}");
		}

		private static void CmdExecuteAndForget(string text)
		{
			using (ConsoleApp console = new ConsoleApp("cmd.exe", string.Empty))
			{
				console.Run();
				console.WriteLine(text);
				console.Stop();
				console.WaitForExit();
			}
		}
	}
}
