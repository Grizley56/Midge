using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Midge.Server.Database
{
	public class OsAccess : INotifyPropertyChanged
	{
		private int _id;
		private bool _canTurnOff;
		private bool _canRestart;
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
		public bool CanTurnOff
		{
			get => _canTurnOff;
			set
			{
				if (value == _canTurnOff) return;
				_canTurnOff = value;
				OnPropertyChanged();
			}
		}
		public bool CanRestart
		{
			get => _canRestart;
			set
			{
				if (value == _canRestart) return;
				_canRestart = value;
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