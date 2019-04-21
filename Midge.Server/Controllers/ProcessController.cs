using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Midge.Server.Communication;
using Midge.Server.Communication.Core;
using Midge.Server.Core;
using Midge.Server.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Midge.Server.Controllers
{
	[MidgeController("process")]
	public class ProcessController: ControllerBase
	{
		private IProcessService _processService;

		public ProcessController(MidgeContext context, IServiceManager serviceManager) 
			: base(context, serviceManager)
		{
			_processService = Services.GetService<IProcessService>();
		}

		[UsedImplicitly]
		[MidgeCommand("currentProcesses")]
		public void CurrentProcesses([MidgeParameter("withSvchost", false)] bool? withSvchost = null)
		{
			if (withSvchost == null)
				withSvchost = true;

			IEnumerable<Process> processes = _processService.CurrentProcesses;

			if (!withSvchost.Value)
				processes = processes.Where(i => i.ProcessName != "svchost");
			
			JArray resp = new JArray(
				from process in processes
				select new JObject(
					new JProperty("process_name", process.ProcessName),
					new JProperty("process_id", process.Id),
					new JProperty("process_ram", process.WorkingSet64)));

			Response = resp;
		}

		[UsedImplicitly]
		[MidgeCommand("kill")]
		public void KillProcess([MidgeParameter("process_id")] int id)
		{
			_processService.Kill(id);
		}
	}
}
