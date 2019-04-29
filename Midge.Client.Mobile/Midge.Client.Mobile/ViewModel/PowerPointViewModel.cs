using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Midge.Client.Mobile.Core;
using Xamarin.Forms;

namespace Midge.Client.Mobile.ViewModel
{
	public class PowerPointViewModel
	{
		public ICommand NextSlideCommand { get; private set; }
		public ICommand PreviousSlideCommand { get; private set; }
		public ICommand StartCommand { get; private set; }

		public PowerPointViewModel()
		{
			NextSlideCommand = new Command(NextSlide);
			PreviousSlideCommand = new Command(PreviousSlide);
			StartCommand = new Command(Play);
		}

		private async void NextSlide()
		{
			await MidgeCore.Instance.Client.Presentation.NextSlide();
		}

		private async void PreviousSlide()
		{
			await MidgeCore.Instance.Client.Presentation.PreviousSlide();
		}

		private async void Play()
		{
			await MidgeCore.Instance.Client.Presentation.Play();
		}
	}
}
