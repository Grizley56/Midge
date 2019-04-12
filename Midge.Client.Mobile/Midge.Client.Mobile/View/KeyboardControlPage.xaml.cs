using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midge.Client.Mobile.MidgeControls;
using Midge.Client.Mobile.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Midge.Client.Mobile.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class KeyboardControlPage : ContentPage
	{
		private KeyboardControlViewModel _viewModel;

		public KeyboardControlPage()
		{
			InitializeComponent();
			_viewModel = new KeyboardControlViewModel();
			
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			MessagingCenter.Send(this, "forceLandscapeLayout");
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			MessagingCenter.Send(this, "preventLandscapeLayout");
		}

		private async void KeyboardTapped(object sender, EventArgs e)
		{
			var button = (MidgeKeyboardButton)sender;
			await _viewModel.SendKey(button.VirtualCode);
		}

		private async void KeyboardPressed(object sender, EventArgs e)
		{
			var button = (MidgeKeyboardButton)sender;
			await _viewModel.KeyDown(button.VirtualCode);
		}

		private async void KeyboardReleased(object sender, EventArgs e)
		{
			var button = (MidgeKeyboardButton)sender;
			await _viewModel.KeyUp(button.VirtualCode);
		}
	}
}