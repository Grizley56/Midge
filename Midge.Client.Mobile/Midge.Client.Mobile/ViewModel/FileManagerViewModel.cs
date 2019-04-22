using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;
using Midge.Client.Mobile.Core;
using Xamarin.Forms;

namespace Midge.Client.Mobile.ViewModel
{
	public class FileManagerViewModel: INotifyPropertyChanged
	{
		private FileEntry _currentEntry;
		private FileEntry _selectedEntry;
		public ObservableCollection<FileEntry> Entries { get; set; }


		public ICommand RemoveSelected { get; private set; }
		public ICommand GoBack { get; private set; }

		public FileEntry CurrentEntry
		{
			get => _currentEntry;
			set
			{
				if (Equals(value, _currentEntry)) return;
				_currentEntry = value;
				OnPropertyChanged();
			}
		}

		public FileEntry SelectedEntry
		{
			get => _selectedEntry;
			set
			{
				if (Equals(value, _selectedEntry)) return;
				_selectedEntry = value;
				OnPropertyChanged();
			}
		}

		public FileManagerViewModel()
		{
			Entries = new ObservableCollection<FileEntry>();

			RemoveSelected = new Command(async () =>
			{
				if (SelectedEntry == null)
					return;

				await MidgeCore.Instance.Client.FileManager.Remove(SelectedEntry.FilePath);
				Entries.Remove(SelectedEntry);
				SelectedEntry = null;
			});

			GoBack = new Command(async () =>
			{
				if (CurrentEntry == null)
					return;

				if (CurrentEntry.Type == FileType.Drive)
				{
					await Initialize();
					return;
				}
				else
				{
					var parentDir = CurrentEntry.FilePath.Split(new[] {'\\', '/'}).ToArray();
					var path = string.Join("\\", parentDir.Take(parentDir.Length - 1));
					var entry = new FileEntry(path, path.Contains('\\') ? FileType.Directory : FileType.Drive, 0);
					await Open(entry);
				}
			});
		}

		public async Task Initialize()
		{
			CurrentEntry = null;
			Entries.Clear();

			var disks = await MidgeCore.Instance.Client.FileManager.GetLogicalDrivesAsync();
			foreach (var disk in disks)
				Entries.Add(new FileEntry(disk.Path, FileType.Drive, 0) {Name = disk.Path});
		}

		public async Task Open(FileEntry fileEntry)
		{
			if (fileEntry.Type == FileType.Directory || fileEntry.Type == FileType.Drive)
			{
				var dir = await MidgeCore.Instance.Client.FileManager.GetDirectoryAsync(fileEntry.FilePath);

				CurrentEntry = fileEntry;

				Entries.Clear();

				foreach (var item in dir.Directories)
					Entries.Add(new FileEntry(item, FileType.Directory, 0));

				foreach (var file in dir.Files)
					Entries.Add(new FileEntry(file.Name, FileType.File, file.Size));
			}
			else
			{
				// TODO: download file
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;


		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class FileEntry
	{
		private string _name;

		public string Name
		{
			get
			{
				if (_name == null)
				{
					return FilePath.Split(new [] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
				}

				return _name;
			}

			set => _name = value;
		}
		
		public string FilePath { get; set; }

		public FileType Type { get; set; }
		public long Size { get; set; }

		public string Icon
		{
			get
			{
				if (Type == FileType.Directory)
					return "Folder";
				else if (Type == FileType.Drive)
					return "Drive";
				else
					return "File";
			}
		}

		public FileEntry()
		{
			
		}

		public FileEntry(string path, FileType type, long size)
		{
			FilePath = path;
			Type = type;
			Size = size;
		}
	}

	public enum FileType
	{
		File,
		Directory,
		Drive
	}

}
