using System.ComponentModel;
using System.Runtime.CompilerServices;
using Midge.Client.Mobile.Annotations;

namespace Midge.Client.Mobile.Model
{
	public class SoundModel: INotifyPropertyChanged
	{
		private int _volume;
		private bool _isMuted;

		public int Volume
		{
			get => _volume;
			set
			{
				if (_volume == value)
					return;

				_volume = value;
				OnPropertyChanged();
			}
		}

		public bool IsMuted
		{
			get => _isMuted;
			set
			{
				if (_isMuted == value)
					return;

				_isMuted = value;
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
