using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Midge.Server.Core
{
	public class JsonMessage
	{
		public string Command { get; }
		public string Controller { get; }
		public Guid? Key { get; }
		public IReadOnlyCollection<Parameter> Parameters => _params;

		private readonly List<Parameter> _params;

		public JsonMessage(string controller, string command, Guid? key, IEnumerable<Parameter> @params)
		{
			Command = command;
			Key = key;
			Controller = controller;
			_params = new List<Parameter>(@params);
		}

		public JsonMessage(string command, string controller, Guid? key = null)
			:this(command, controller, key,new Parameter[0])
		{
			
		}
	}
}
