using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MidgeContract
{
	[DataContract]
	public class ProcessInfo
	{
		[DataMember]
		public string ProcessName { get; private set; }
		[DataMember]
		public int ProcessId { get; private set; }
		[DataMember]
		public long WorkingSet { get; private set; }

		public ProcessInfo(string processName, int processId, long workingSet)
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
