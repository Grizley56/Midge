using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MidgeService
{
	public partial class MidgeServer : ServiceBase
	{
		private ServiceHost _host;

		public MidgeServer()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			_host?.Close();
			_host = new ServiceHost(typeof(Midge));

			_host.Open();
		}

		protected override void OnStop()
		{
			_host?.Close();
			_host = null;
		}
	}
}
