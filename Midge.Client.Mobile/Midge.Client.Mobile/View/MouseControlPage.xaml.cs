using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midge.Client.Mobile.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Midge.Client.Mobile.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MouseControlPage : ContentPage
	{
		private MouseControlViewModel _viewModel;

		public MouseControlPage()
		{
			InitializeComponent();

			_viewModel = new MouseControlViewModel();
			BindingContext = _viewModel;
		}

		private Point _previousPosition = Point.Zero;


		private async void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
		{
			Point currentPosition = new Point(e.TotalX, e.TotalY);

			if (currentPosition.IsEmpty)
			{
				_previousPosition = Point.Zero;
				return;
			}

			Point offset = new Point(currentPosition.X - _previousPosition.X, currentPosition.Y - _previousPosition.Y);
			Debug.WriteLine("offset: " + offset);

			_previousPosition = currentPosition;

			await _viewModel.MoveMouseCursor(offset);
		}

		private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			await _viewModel.LeftButtonClick();
		}

	}
}