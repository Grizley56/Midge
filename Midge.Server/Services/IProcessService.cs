using System.Diagnostics;

namespace Midge.Server.Services
{
	public interface IProcessService
	{
		Process[] CurrentProcesses { get; }
		void Kill(int id);
	}
}
