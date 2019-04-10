using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midge.Server.Services;

namespace Midge.Server.Windows.Services
{
	public class ProcessService : IProcessService
	{
		public Process[] CurrentProcesses => Process.GetProcesses();

		public void Kill(int processId)
		{
			Process process = Process.GetProcessById(processId);
			process.Kill();
		}
	}
}
