using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Midge.Server.Database
{
	[Table("StorageAccess")]
	public class StorageAccess : INotifyPropertyChanged
	{
		private int _id;
		private int _userAccessId;
		private bool _canView;
		private bool _canDeleteFile;
		private bool _canMoveFile;
		private bool _canCopyFile;
		private bool _canDownloadFile;

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


		public bool CanView
		{
			get => _canView;
			set
			{
				if (value == _canView) return;
				_canView = value;
				OnPropertyChanged();
			}
		}

		public bool CanDeleteFile
		{
			get => _canDeleteFile;
			set
			{
				if (value == _canDeleteFile) return;
				_canDeleteFile = value;
				OnPropertyChanged();
			}
		}

		public bool CanMoveFile
		{
			get => _canMoveFile;
			set
			{
				if (value == _canMoveFile) return;
				_canMoveFile = value;
				OnPropertyChanged();
			}
		}

		public bool CanCopyFile
		{
			get => _canCopyFile;
			set
			{
				if (value == _canCopyFile) return;
				_canCopyFile = value;
				OnPropertyChanged();
			}
		}

		public bool CanDownloadFile
		{
			get => _canDownloadFile;
			set
			{
				if (value == _canDownloadFile) return;
				_canDownloadFile = value;
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