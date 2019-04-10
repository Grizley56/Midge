using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Midge.API.Models
{
	[Serializable]
	public class ProcessModel
	{
		[JsonProperty("process_name")]
		public string ProcessName { get; private set; }
		[JsonProperty("process_id")]
		public int ProcessId { get; private set; }
		[JsonProperty("process_ram")]
		public long WorkingSet { get; private set; }

		public ProcessModel(string processName, int processId, long workingSet)
		{
			ProcessName = processName;
			ProcessId = processId;
			WorkingSet = workingSet;
		}

		public override string ToString()
		{
			return ProcessName;
		}
	}
}
