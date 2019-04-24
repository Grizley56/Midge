using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Midge.Server.Database
{
	[Table("Users")]
	public class User : INotifyPropertyChanged
	{
		private int _id;
		private string _login;
		private string _password;
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

		public string Login
		{
			get => _login;
			set
			{
				if (value == _login) return;
				_login = value;
				OnPropertyChanged();
			}
		}

		public string Password
		{
			get => _password;
			set
			{
				if (value == _password) return;
				_password = value;
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

		[ForeignKey(typeof(AccessToken))]
		public int AccessTokenId { get; set; }



		[OneToOne]
		public AccessToken AccessToken { get; set; }

		[ManyToOne(CascadeOperations = CascadeOperation.CascadeRead)]
		public UserAccess UserAccess { get; set; }


		public static User Empty => new User() {UserAccess = UserAccess.Empty};

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}