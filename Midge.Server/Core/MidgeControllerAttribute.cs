using System;

namespace Midge.Server.Core
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class MidgeControllerAttribute: Attribute
	{
		public string ControllerName { get; }

		public MidgeControllerAttribute(string controllerName)
		{
			ControllerName = controllerName;
		}
	}
}
