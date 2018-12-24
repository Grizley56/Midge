using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace MidgeService
{
	[RunInstaller(true)]
	public partial class Installer : System.Configuration.Install.Installer
	{
		private readonly ServiceProcessInstaller _process;
		private readonly ServiceInstaller _service;

		public Installer()
		{
			_process = new ServiceProcessInstaller {Account = ServiceAccount.LocalSystem};
			_service = new ServiceInstaller {ServiceName = "Midge", StartType = ServiceStartMode.Automatic };
			Installers.Add(_process);
			Installers.Add(_service);
		}
	}
}
