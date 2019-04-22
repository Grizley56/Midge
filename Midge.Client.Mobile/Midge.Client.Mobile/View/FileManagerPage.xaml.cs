using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midge.Client.Mobile.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Midge.Client.Mobile.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FileManagerPage : ContentPage
	{
		private static FileManagerViewModel _viewModel = new FileManagerViewModel();


		public FileManagerPage()
		{
			InitializeComponent();

			BindingContext = _viewModel;

			_viewModel.Initialize();
		}

		private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			await _viewModel.Open(e.Item as FileEntry);
		}



	}
}