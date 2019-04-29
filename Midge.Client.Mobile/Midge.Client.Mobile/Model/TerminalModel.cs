using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;

namespace Midge.Client.Mobile.Model
{
	public class TerminalModel: INotifyPropertyChanged
	{
		private string _outputText;

		public string OutputText
		{
			get => _outputText;
			set
			{
				if (value == _outputText) return;
				_outputText = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
