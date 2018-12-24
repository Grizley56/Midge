using Midge.Core;
using Midge.ViewModel;
using Xamarin.Forms.Xaml;

namespace Midge.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainTabbedPage : Xamarin.Forms.TabbedPage
	{
		public MainTabbedPage()
		{
			InitializeComponent();
		}
	}
}