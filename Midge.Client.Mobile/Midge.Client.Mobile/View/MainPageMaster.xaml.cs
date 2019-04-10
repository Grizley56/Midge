using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Midge.Client.Mobile.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Midge.Client.Mobile.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPageMaster : ContentPage
	{
		public ListView ListView;

		public MainPageMaster()
		{
			InitializeComponent();

			BindingContext = new MainPageMasterViewModel();
			ListView = MenuItemsListView;
		}
	}
}