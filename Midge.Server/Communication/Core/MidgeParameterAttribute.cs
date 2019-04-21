using System;

namespace Midge.Server.Communication.Core
{
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
	public class MidgeParameterAttribute: Attribute
	{
		public string Name { get; }
		public bool IsRequired { get; }
		public MidgeParameterAttribute(string name, bool isRequired = true)
		{
			Name = name;
			IsRequired = IsRequired;
		}
	}
}
