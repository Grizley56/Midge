using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Midge.Server.Database
{
	[Table("ProcessAccess")]
	public class ProcessAccess : INotifyPropertyChanged
	{
		private int _id;
		private bool _canViewProcesses;
		private bool _canKillProcesses;
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
		public bool CanViewProcesses
		{
			get => _canViewProcesses;
			set
			{
				if (value == _canViewProcesses) return;
				_canViewProcesses = value;
				OnPropertyChanged();
			}
		}
		public bool CanKillProcesses
		{
			get => _canKillProcesses;
			set
			{
				if (value == _canKillProcesses) return;
				_canKillProcesses = value;
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