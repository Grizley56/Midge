using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.Server.Utils
{
	public static class AttributeHelper
	{
		public static IEnumerable<Type> GetClassesWithAttribute<T>() where T: Attribute
		{
			return
				from assembly in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
				from type in assembly.GetTypes()
				let attributes = type.GetCustomAttributes(typeof(T), true)
				where attributes != null && attributes.Length > 0
				select type;
		}
	}
}
