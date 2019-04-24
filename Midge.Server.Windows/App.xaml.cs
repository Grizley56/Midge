using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Midge.Server;
using Midge.Server.Windows;
using Midge.Server.Windows.Utils;

namespace Midge.Client.Mobile.Server.Windows
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static MidgeServer Server { get; private set; }

		protected override void OnStartup(StartupEventArgs e)
		{
			//AudioCapturer.Init();

			Server = new MidgeServer(8733, CertificateHelper.GenerateCertificate("Midge"));

			base.OnStartup(e);
		}


	}
}
