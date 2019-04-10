using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midge.Server.Core;
using Newtonsoft.Json.Linq;

namespace Midge.Server.Controllers
{
	public abstract class ControllerBase
	{
		public IContext Context { get; set; }
		public IServiceManager Services { get; set; }

		protected ControllerBase(IContext context, IServiceManager serviceManager)
		{
			Services = serviceManager;
			Context = context;
		}


		public JToken Response { get; protected set; }
	}
}
