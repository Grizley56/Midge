using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Midge.API.Models;
using Midge.Client.Mobile.Annotations;
using Midge.Client.Mobile.Core;
//using MidgeContract;
using Xamarin.Forms;

namespace Midge.Client.Mobile.ViewModel
{
	public class ProcessViewModel: INotifyPropertyChanged
	{
		private ObservableCollection<ProcessModel> _processes;

		private readonly object _lock = new object();

		private ProcessModel _selectedProcess;

		public ICommand RefreshProcessList { get; private set; }
		public ICommand KillSelectedProcess { get; private set; }

		public ProcessModel SelectedProcess
		{
			get => _selectedProcess;
			set
			{
				_selectedProcess = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<ProcessModel> Processes
		{
			get
			{
				lock (_lock)
					return _processes;
			}
			private set
			{
				lock (_lock)
					_processes = value;

				OnPropertyChanged();
			}
		}

		public ProcessViewModel()
		{
			Processes = new ObservableCollection<ProcessModel>();

			RefreshProcessList = new Command(async () =>
			{
				ProcessModel[] processes = null;

				processes = await MidgeCore.Instance.Client.Process.GetCurrentProcesses();

				if (processes == null)
					return;
				
				Processes = new ObservableCollection<ProcessModel>(processes.ToArray().OrderByDescending(i => i.WorkingSet));
			});

			KillSelectedProcess = new Command(async () =>
			{
				if (SelectedProcess == null)
					return;

				await MidgeCore.Instance.Client.Process.Kill(SelectedProcess.ProcessId);

				Processes.Remove(SelectedProcess);
			});
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
