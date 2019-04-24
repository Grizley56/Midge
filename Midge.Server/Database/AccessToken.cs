using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Midge.Server.Database
{
	[Table("AccessTokens")]
	public class AccessToken : INotifyPropertyChanged
	{
		private int _id;
		private string _value;
		private int _userId;
		private DateTime _createdTime;

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
		public string Value
		{
			get => _value;
			set
			{
				if (value == _value) return;
				_value = value;
				OnPropertyChanged();
			}
		}
		public DateTime CreatedTime
		{
			get => _createdTime;
			set
			{
				if (value.Equals(_createdTime)) return;
				_createdTime = value;
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
		[OneToOne]
		public User User { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}