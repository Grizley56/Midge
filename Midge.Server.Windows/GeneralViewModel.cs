using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using Midge.Client.Mobile.Server.Windows;
using Midge.Server.Core;
using Midge.Server.Database;
using Midge.Server.Web;
using Midge.Server.Windows.Services;
using SQLiteNetExtensions.Extensions;

namespace Midge.Server.Windows
{
	public class GeneralViewModel: INotifyPropertyChanged
	{
		private UserAccess _selectedAccess;
		private User _selectedUser;
		private ServerState _serverState;
		private IDependencyStorage _dependencyStorage = new DependencySolverPlug();

		public ObservableCollection<UserAccess> Accesses { get; set; }
		public ObservableCollection<User> Users { get; set; }
		public ObservableCollection<Log> Logs { get; set; }

		public ILogger Logger { get; set; }

		public UserAccess SelectedAccess
		{
			get => _selectedAccess;
			set
			{
				if (Equals(value, _selectedAccess)) return;
				_selectedAccess = value;
				OnPropertyChanged();
			}
		}

		public User SelectedUser
		{
			get => _selectedUser;
			set
			{
				if (Equals(value, _selectedUser)) return;
				_selectedUser = value;
				OnPropertyChanged();
			}
		}

		public ICommand AccessSaveCommand { get; private set; }
		public ICommand AccessUpdateCommand { get; private set; }

		public ICommand UserSaveCommand { get; private set; }
		public ICommand UserUpdateCommand { get; private set; }

		public ICommand RemoveUserAccessCommand { get; private set; }
		public ICommand RemoveUserCommand { get; private set; }

		public ICommand StartServerCommand { get; private set; }
		public ICommand StopServerCommand { get; private set; }
		public ICommand RestartServerCommand { get; private set; }


		public ServerState ServerState
		{
			get => _serverState;
			set
			{
				if (value == _serverState) return;
				_serverState = value;
				OnPropertyChanged();
			}
		}

		public GeneralViewModel()
		{
			SelectedAccess = UserAccess.Empty;
			SelectedUser = User.Empty;

			App.Server.InternalTcp.ServerStateChanged += InternalTcp_ServerStateChanged;

			Init();

			AccessSaveCommand = new RelayCommand(SaveAccess);
			AccessUpdateCommand = new RelayCommand(UpdateAccess);
			UserSaveCommand = new RelayCommand(SaveUser);
			UserUpdateCommand = new RelayCommand(UpdateUser);
			RemoveUserAccessCommand = new RelayCommand(RemoveAccess);
			RemoveUserCommand = new RelayCommand(RemoveUser);

			StopServerCommand = new RelayCommand(StopServer);
			StartServerCommand = new RelayCommand(StartServer);
			RestartServerCommand = new RelayCommand(RestartServer);
		}

		private void InternalTcp_ServerStateChanged(object sender, Web.ServerStateEventArgs e)
		{
			ServerState = e.State;

			Logger?.Log($"SERVER {e.State.ToString().ToUpper()}");
		}

		public void Init()
		{
			Accesses = new ObservableCollection<UserAccess>(App.Server.Context.Connection.GetAllWithChildren<UserAccess>());
			Users = new ObservableCollection<User>(App.Server.Context.Connection.GetAllWithChildren<User>());
			Logs = new ObservableCollection<Log>(App.Server.Context.Connection.GetAllWithChildren<Log>());

			ServerState = App.Server.InternalTcp.State;
		}

		public async void StopServer(object param)
		{
			await App.Server.Stop();
		}

		public async void StartServer(object param)
		{
			App.Server.DependencyStorage = _dependencyStorage;
			await App.Server.Start();
		}

		public async void RestartServer(object param)
		{
			await App.Server.Stop();

			App.Server.DependencyStorage = _dependencyStorage;
			await App.Server.Start();
		}

		private void SaveAccess(object param)
		{
			App.Server.Context.Connection.InsertWithChildren(SelectedAccess, true);
			Accesses.Add(SelectedAccess);
			SelectedAccess = null;
			SelectedAccess = UserAccess.Empty;
		}

		private void UpdateAccess(object param)
		{
			App.Server.Context.Connection.UpdateWithChildren(SelectedAccess);
			SelectedAccess = null;
			SelectedAccess = UserAccess.Empty;
		}

		private void RemoveAccess(object param)
		{
			if (param == null || !(param is UserAccess access))
				return;

			App.Server.Context.Connection.Delete(access);
			Accesses.Remove(access);

			SelectedAccess = null;
			SelectedAccess = UserAccess.Empty;
		}

		private void SaveUser(object param)
		{
			App.Server.Context.Connection.InsertWithChildren(SelectedUser, false);
			Users.Add(SelectedUser);
			SelectedUser = null;
			SelectedUser = User.Empty;
		}

		private void UpdateUser(object param)
		{
			App.Server.Context.Connection.UpdateWithChildren(SelectedUser);
			SelectedUser = null;
			SelectedUser = User.Empty;
		}

		private void RemoveUser(object param)
		{
			if (param == null || !(param is User user))
				return;

			App.Server.Context.Connection.Delete(user);
			Users.Remove(user);

			SelectedUser = null;
			SelectedUser = User.Empty;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void UpdateLogs()
		{
			//Logs.Clear();
			//var logs = (App.Server.Context.Connection.GetAllWithChildren<Log>());
			//foreach (var log in logs)
			//	Logs.Add(log);
		}
	}
}
