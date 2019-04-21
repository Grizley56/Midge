using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Midge.Server.Windows;

namespace Midge.Client.Mobile.Server.Windows
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			AudioCapturer.Init();
			base.OnStartup(e);
		}
	}
}
