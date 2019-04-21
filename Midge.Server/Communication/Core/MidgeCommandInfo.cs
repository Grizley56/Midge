using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Midge.Server.Communication.Core
{
	public class MidgeCommandInfo
	{
		public string CommandName { get; }
		public MethodInfo MethodInfo { get; }
		public IEnumerable<MidgeParameterInfo> Params { get; }
		public bool IsAsync { get; }

		public MidgeCommandInfo(string commandName, MethodInfo methodInfo, IEnumerable<MidgeParameterInfo> @params)
		{
			CommandName = commandName;
			MethodInfo = methodInfo;
			Params = @params;

			if (MethodInfo.ReturnType == typeof(Task))
				IsAsync = true;
		}
	}
}