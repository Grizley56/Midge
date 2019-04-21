using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midge.Server.Core;
using Midge.Server.Services;

namespace Midge.Server.Windows.Services
{
	public class DependencySolver : IDependencyStorage
	{
		public IProcessService ProcessService { get; }
		public IVolumeService VolumeService { get; }
		public IControlService ControlService { get; }
		public IAudioStreamService AudioStreamService { get; }

		public DependencySolver()
		{
			ProcessService = new ProcessService();
			VolumeService = new VolumeService();
			ControlService = new ControlService();
			AudioStreamService = new AudioStreamService();
		}
	}
}
