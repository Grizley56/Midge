using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Acr.UserDialogs;
using Midge.Client.Mobile.Core;
using Xamarin.Forms;

namespace Midge.Client.Mobile.ViewModel
{
	public class SystemViewModel
	{
		public ICommand TurnOffCommand { get; private set; }
		public ICommand RestartCommand { get; private set; }

		public ICommand TurnOffByTimerCommand { get; private set; }
		public ICommand RestartByTimerCommand { get; private set; }

		public SystemViewModel()
		{
			TurnOffCommand = new Command(TurnComputerOff);
			RestartCommand = new Command(RestartComputer);
			TurnOffByTimerCommand = new Command<TimePicker>(TurnOffByTimer);
			RestartByTimerCommand = new Command<TimePicker>(RestartByTimer);
		}

		private void TurnComputerOff()
		{
			TurnOff(TimeSpan.Zero);
			UserDialogs.Instance.AlertAsync($"Computer will be turn off right now!", "OK");
		}

		private void RestartComputer()
		{
			Restart(TimeSpan.Zero);
			UserDialogs.Instance.AlertAsync($"Computer will be restart right now!", "OK");
		}

		private void RestartByTimer(TimePicker timePicker)
		{
			UserDialogs.Instance.TimePrompt(new TimePromptConfig()
			{
				OnAction =  async (time) =>
				{
					if (time.Ok)
					{
						var delay = GetDelay(time.SelectedTime);
						Restart(delay);

						await UserDialogs.Instance.AlertAsync($"Computer will be restart after {delay: HH:mm:ss}", "OK");
					}
				}
			});
		}

		private TimeSpan GetDelay(TimeSpan turnOffTime)
		{
			var now = DateTime.Now.TimeOfDay;

			if (turnOffTime > now)
				return turnOffTime - now;

			var elapsed = TimeSpan.FromHours(24) - now;
			return elapsed + turnOffTime;
		}

		private void TurnOffByTimer(TimePicker timePicker)
		{
			UserDialogs.Instance.TimePrompt(new TimePromptConfig()
			{
				OnAction = async (time) =>
				{
					if (time.Ok)
					{
						var delay = GetDelay(time.SelectedTime);
						TurnOff(delay);
						await UserDialogs.Instance.AlertAsync($"Computer will be turn off after {delay: HH:mm:ss}", "OK");
					}
				}
			});
		}

		public void TurnOff(TimeSpan delay = default)
		{
			MidgeCore.Instance.Client.System.TurnOff(delay);
		}

		public void Restart(TimeSpan delay = default)
		{
			MidgeCore.Instance.Client.System.TurnOff(delay);
		}
	}
}
