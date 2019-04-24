using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Midge.Server.Database
{
	[Table("UserAccesses")]
	public class UserAccess : INotifyPropertyChanged
	{
		private string _accessName;
		private SoundAccess _soundAccess;
		private ProcessAccess _processAccess;
		private EmulatorsAccess _emulatorsAccess;
		private OsAccess _osAccess;
		private StorageAccess _storageAccess;

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		
		public string AccessName
		{
			get => _accessName;
			set
			{
				if (value == _accessName) return;
				_accessName = value;
				OnPropertyChanged();
			}
		}


		[ForeignKey(typeof(StorageAccess))]
		public int StorageAccessId { get; set; }

		[ForeignKey(typeof(OsAccess))]
		public int OsAccessId { get; set; }

		[ForeignKey(typeof(EmulatorsAccess))]
		public int EmulatorAccessId { get; set; }

		[ForeignKey(typeof(ProcessAccess))]
		public int ProcessAccessId { get; set; }

		[ForeignKey(typeof(SoundAccess))]
		public int SoundAccessId { get; set; }

		[OneToOne(CascadeOperations = CascadeOperation.All)]
		public SoundAccess SoundAccess
		{
			get => _soundAccess;
			set
			{
				if (Equals(value, _soundAccess)) return;
				_soundAccess = value;
				OnPropertyChanged();
			}
		}

		[OneToOne(CascadeOperations = CascadeOperation.All)]
		public ProcessAccess ProcessAccess
		{
			get => _processAccess;
			set
			{
				if (Equals(value, _processAccess)) return;
				_processAccess = value;
				OnPropertyChanged();
			}
		}

		[OneToOne(CascadeOperations = CascadeOperation.All)]
		public EmulatorsAccess EmulatorsAccess
		{
			get => _emulatorsAccess;
			set
			{
				if (Equals(value, _emulatorsAccess)) return;
				_emulatorsAccess = value;
				OnPropertyChanged();
			}
		}

		[OneToOne(CascadeOperations = CascadeOperation.All)]
		public OsAccess OsAccess
		{
			get => _osAccess;
			set
			{
				if (Equals(value, _osAccess)) return;
				_osAccess = value;
				OnPropertyChanged();
			}
		}

		[OneToOne(CascadeOperations = CascadeOperation.All)]
		public StorageAccess StorageAccess
		{
			get => _storageAccess;
			set
			{
				if (Equals(value, _storageAccess)) return;
				_storageAccess = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


		public static UserAccess Empty =>
			new UserAccess()
			{
				EmulatorsAccess = new EmulatorsAccess(),
				OsAccess = new OsAccess(),
				ProcessAccess = new ProcessAccess(),
				SoundAccess = new SoundAccess(),
				StorageAccess = new StorageAccess()
			};

		public override string ToString()
		{
			return AccessName;
		}
	}
}