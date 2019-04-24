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
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace Midge.Server.Windows
{
	/// <summary>
	/// Interaction logic for GeneralWindow.xaml
	/// </summary>
	public partial class GeneralWindow : MetroWindow
	{
		private GeneralViewModel _viewModel => (GeneralViewModel)DataContext;

		public GeneralWindow()
		{
			InitializeComponent();
		}

		

		private void MetroTabItem_Selected(object sender, RoutedEventArgs e)
		{
			_viewModel?.UpdateLogs();
		}

		private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
		{
			_viewModel.Logger = new RichTextBoxLogAdapter(LastActionsRtb);
		}
	}
}
