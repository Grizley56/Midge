using Midge.Server.Core;
using Newtonsoft.Json.Linq;

namespace Midge.Server.Communication.Core
{
	public abstract class ControllerBase
	{
		public MidgeContext Context { get; set; }
		public IServiceManager Services { get; set; }

		protected ControllerBase(MidgeContext context, IServiceManager serviceManager)
		{
			Services = serviceManager;
			Context = context;
		}


		public JToken Response { get; protected set; }
	}
}
