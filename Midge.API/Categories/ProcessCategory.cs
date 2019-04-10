using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midge.API.Models;

namespace Midge.API.Categories
{
	public class ProcessCategory: Category
	{
		public ProcessCategory(IMidgeInvoke midgeInvoke) : base(midgeInvoke)
		{

		}

		public async Task<ProcessModel[]> GetCurrentProcesses(bool svcHost = false)
		{
			var result = await MidgeInvoke.SendAndWaitAsync<Collection<ProcessModel>>("process.currentProcesses",
				new MidgeParameters
				{
					{"withSvchost", svcHost}
				});
			
			return result.ToArray();
		}

		public async Task Kill(int processId)
		{
			await MidgeInvoke.SendAsync("process.kill", new MidgeParameters
			{
				{"process_id", processId}
			});
		}
	}
}
