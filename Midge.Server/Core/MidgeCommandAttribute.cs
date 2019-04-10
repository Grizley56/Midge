using System;

namespace Midge.Server.Core
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class MidgeCommandAttribute: Attribute
	{
		public string CommandName { get; }

		public MidgeCommandAttribute(string commandName)
		{
			CommandName = commandName;
		}
	}
}
