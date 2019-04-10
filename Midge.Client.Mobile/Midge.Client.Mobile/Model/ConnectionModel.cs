using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Midge.Client.Mobile.Annotations;
using Midge.Client.Mobile.Core;

namespace Midge.Client.Mobile.Model
{
	public class ConnectionModel: INotifyPropertyChanged
	{
		private ConnectionState _state;

		public ConnectionState State
		{
			get => _state;
			set
			{
				if (_state == value)
					return;

				_state = value;
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
