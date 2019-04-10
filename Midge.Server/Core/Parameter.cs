using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Midge.Server.Core
{
	public class Parameter
	{
		public string Name { get; }
		public JToken Value { get; }

		public Parameter(string name, JToken value)
		{
			Name = name;
			Value = value;
		}
	}
}
