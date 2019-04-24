using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Midge.Server.Database
{
	[Table("SoundAccess")]
	public class SoundAccess : INotifyPropertyChanged
	{
		private int _id;
		private bool _canChangeVolume;
		private bool _canChangeMute;
		private bool _canCaptureAudio;
		private int _userAccessId;

		[PrimaryKey, AutoIncrement]
		public int Id
		{
			get => _id;
			set
			{
				if (value == _id) return;
				_id = value;
				OnPropertyChanged();
			}
		}

		public bool CanChangeVolume
		{
			get => _canChangeVolume;
			set
			{
				if (value == _canChangeVolume) return;
				_canChangeVolume = value;
				OnPropertyChanged();
			}
		}
		public bool CanChangeMute
		{
			get => _canChangeMute;
			set
			{
				if (value == _canChangeMute) return;
				_canChangeMute = value;
				OnPropertyChanged();
			}
		}
		public bool CanCaptureAudio
		{
			get => _canCaptureAudio;
			set
			{
				if (value == _canCaptureAudio) return;
				_canCaptureAudio = value;
				OnPropertyChanged();
			}
		}

		[ForeignKey(typeof(UserAccess))]
		public int UserAccessId
		{
			get => _userAccessId;
			set
			{
				if (value == _userAccessId) return;
				_userAccessId = value;
				OnPropertyChanged();
			}
		}

		[OneToOne]
		public UserAccess UserAccess { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}