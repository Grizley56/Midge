using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.API.Categories
{
	public class SystemCategory: Category
	{
		public SystemCategory(IMidgeInvoke midgeInvoke) : base(midgeInvoke)
		{

		}


		public Task TurnOff(TimeSpan delay)
		{
			return MidgeInvoke?.SendAsync("system.off", new MidgeParameters
			{
				{ "delay", (int)delay.TotalSeconds }
			});
		}

		public Task Restart(TimeSpan delay)
		{
			return MidgeInvoke?.SendAsync("system.restart", new MidgeParameters
			{
				{ "delay", (int)delay.TotalSeconds }
			});
		}
	}
}
