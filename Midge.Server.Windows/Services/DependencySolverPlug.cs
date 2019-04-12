using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midge.Server.Core;
using Midge.Server.Services;

namespace Midge.Server.Windows.Services
{
	public class DependencySolverPlug : IDependencyStorage
	{
		public IProcessService ProcessService { get; }
		public IVolumeService VolumeService { get; }
		public IControlService ControlService { get; }

		public DependencySolverPlug()
		{
			ProcessService = new ProcessService();
			VolumeService = new VolumeServicePlug();
			ControlService = new ControlService();
		}
	}

	public class VolumeServicePlug : IVolumeService
	{
		public int Volume { get; set; } = 100;
		public bool IsMute { get; set; }
	}
}
