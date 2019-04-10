using System;
using System.Collections.Generic;
using System.Text;
using Midge.Server.Services;

namespace Midge.Server.Core
{
	public interface IDependencyStorage
	{
		IProcessService ProcessService { get; }
		IVolumeService VolumeService { get; }
	}
}
