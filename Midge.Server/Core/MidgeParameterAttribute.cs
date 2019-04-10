using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.Server.Core
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
