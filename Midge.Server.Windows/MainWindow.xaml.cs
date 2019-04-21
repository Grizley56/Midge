using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Midge.Server;
using Midge.Server.Core;
using Midge.Server.Windows.Services;
using Midge.Server.Windows.Utils;

namespace Midge.Client.Mobile.Server.Windows
{
	public partial class MainWindow : Window
	{
		private MidgeServer _server;
		private IDependencyStorage _dep = new DependencySolverPlug();

		public MainWindow()
		{
			InitializeComponent();
			_server = new MidgeServer(8733, CertificateHelper.GenerateCertificate("Midge"));
			DataContext = _server;
		}

		private async void Start(object sender, RoutedEventArgs e)
		{
			_server.DependencyStorage = _dep;
			await _server.Start();
		}

		private async void Stop(object sender, RoutedEventArgs e)
		{
			await _server.Stop();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{

		}
	}
}
