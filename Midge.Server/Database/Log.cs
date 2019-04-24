using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Midge.Server.Database
{
	[Table("Logs")]
	public class Log : INotifyPropertyChanged
	{
		private int _userId;
		private string _message;
		private int _id;
		private DateTime _time;

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
		public DateTime Time
		{
			get => _time;
			set
			{
				if (value.Equals(_time)) return;
				_time = value;
				OnPropertyChanged();
			}
		}
		public string Message
		{
			get => _message;
			set
			{
				if (value == _message) return;
				_message = value;
				OnPropertyChanged();
			}
		}

		[ForeignKey(typeof(User))]
		public int UserId
		{
			get => _userId;
			set
			{
				if (value == _userId) return;
				_userId = value;
				OnPropertyChanged();
			}
		}

		[OneToOne(CascadeOperations = CascadeOperation.CascadeRead)]
		public User User { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}