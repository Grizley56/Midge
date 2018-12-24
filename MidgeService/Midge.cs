using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AudioSwitcher.AudioApi.CoreAudio;
using MidgeContract;

namespace MidgeService
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class Midge : IMidge
	{
		private readonly CoreAudioController _audioController;

		public Midge()
		{
			_audioController = new CoreAudioController();
		}

		public void SetVolume(int value)
		{
			if (value < 0 || value > 100)
				throw new ArgumentOutOfRangeException(nameof(value));
				
			_audioController.DefaultPlaybackDevice.Volume = value;
		}

		public int GetVolume()
		{
			return (int)_audioController.DefaultPlaybackDevice.Volume;
		}

		public void SetMute(bool value)
		{
			_audioController.DefaultPlaybackDevice.Mute(value);
		}

		public bool IsMute()
		{
			return _audioController.DefaultPlaybackDevice.IsMuted;
		}

		public int StartProcess(string name, params string[] args)
		{
			throw new NotImplementedException();
		}

		public bool KillProcess(int id)
		{
			Process process;

			try
			{
				process = Process.GetProcessById(id);
			}
			catch
			{
				return false;
			}

			try
			{
				process.Kill();
			}
			catch
			{
				return false;
			}

			return true;
		}

		public IEnumerable<ProcessInfo> GetProcesses(bool withSvchosts = false)
		{
			IEnumerable<Process> processes = Process.GetProcesses();

			if(!withSvchosts)
				processes = processes.Where(i => i.ProcessName != "svchost");

			return processes.Select(i => new ProcessInfo(i.ProcessName, i.Id, i.WorkingSet64));
		}

		public void IsConnected() { }
	}
}
