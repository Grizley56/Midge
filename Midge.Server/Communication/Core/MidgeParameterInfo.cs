using System;

namespace Midge.Server.Communication.Core
{
	public class MidgeParameterInfo
	{
		public Type ParamType { get; }
		public bool IsRequired { get; }
		public int Order { get; }
		public string Name { get; }
		public MidgeParameterInfo(Type paramType, string name, bool isRequired, int order)
		{
			ParamType = paramType;
			IsRequired = isRequired;
			Order = order;
			Name = name;
		}
	}
}