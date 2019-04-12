using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Midge.Client.Mobile.View;

namespace Midge.Client.Mobile.ViewModel
{
	class MainPageMasterViewModel : INotifyPropertyChanged
	{
		public ObservableCollection<MainPageMenuItem> MenuItems { get; set; }

		public MainPageMasterViewModel()
		{
			MenuItems = new ObservableCollection<MainPageMenuItem>(new[]
			{
				new MainPageMenuItem { Id = 0, Title = "Sound", TargetType = typeof(View.SoundPage), Image = "Sound.png" },
				new MainPageMenuItem { Id = 1, Title = "Processes", TargetType = typeof(ProcessPage), Image = "Processes.png" },
				new MainPageMenuItem { Id = 2, Title = "Settings", TargetType = typeof(View.SettingsPage), Image= "Settings.png" },
				new MainPageMenuItem { Id = 3, Title = "Terminal", TargetType = typeof(TerminalPage), Image = "Terminal.png" },
				new MainPageMenuItem { Id = 4, Title = "Mouse", TargetType = typeof(MouseControlPage), Image = "Mouse.png" },
				new MainPageMenuItem { Id = 5, Title = "Keyboard", TargetType = typeof(KeyboardControlPage), Image = "Keyboard.png" }
			});
		}

		#region INotifyPropertyChanged Implementation
		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged == null)
				return;

			PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
