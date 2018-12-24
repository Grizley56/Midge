using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using Midge.Annotations;

namespace Midge.Model
{
	
	public class ConnectionModel: INotifyPropertyChanged
	{
		private CommunicationState _state;

		public CommunicationState State
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
