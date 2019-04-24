using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Midge.Server.Database
{
	[Table("EmulatorAccess")]
	public class EmulatorsAccess : INotifyPropertyChanged
	{
		private int _id;
		private bool _canUseMouse;
		private bool _canUseKeyboard;
		private bool _canUseTerminal;
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
		public bool CanUseMouse
		{
			get => _canUseMouse;
			set
			{
				if (value == _canUseMouse) return;
				_canUseMouse = value;
				OnPropertyChanged();
			}
		}
		public bool CanUseKeyboard
		{
			get => _canUseKeyboard;
			set
			{
				if (value == _canUseKeyboard) return;
				_canUseKeyboard = value;
				OnPropertyChanged();
			}
		}
		public bool CanUseTerminal
		{
			get => _canUseTerminal;
			set
			{
				if (value == _canUseTerminal) return;
				_canUseTerminal = value;
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