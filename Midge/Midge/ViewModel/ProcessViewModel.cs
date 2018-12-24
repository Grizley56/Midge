using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Midge.Annotations;
using Midge.Core;
using MidgeContract;
using Xamarin.Forms;

namespace Midge.ViewModel
{
	public class ProcessViewModel: INotifyPropertyChanged
	{
		private ObservableCollection<ProcessInfo> _processes;

		private readonly object _lock = new object();

		private ProcessInfo _selectedProcess;

		public ICommand RefreshProcessList { get; private set; }
		public ICommand KillSelectedProcess { get; private set; }

		public ProcessInfo SelectedProcess
		{
			get => _selectedProcess;
			set
			{
				_selectedProcess = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<ProcessInfo> Processes
		{
			get
			{
				lock(_lock)
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
			Processes = new ObservableCollection<ProcessInfo>();

			RefreshProcessList = new Command(() =>
			{
				IEnumerable<ProcessInfo> processes = null;

				if (!ReconnectableChannel<IMidge>.MidgeInstance.Invoke(i => processes = i.GetProcesses()))
					return;

				Debug.Assert(processes != null);
				Processes = new ObservableCollection<ProcessInfo>(processes.ToArray().OrderByDescending(i => i.WorkingSet));
			});

			KillSelectedProcess = new Command(() =>
			{
				if (SelectedProcess == null)
					return;

				if (!ReconnectableChannel<IMidge>.MidgeInstance.Invoke(i => i.KillProcess(SelectedProcess.ProcessId)))
					return;

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
