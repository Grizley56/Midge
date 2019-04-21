using System;
using System.Collections.Generic;

namespace Midge.Server.Communication
{
	public class ClientMessage
	{
		public string Command { get; }
		public string Controller { get; }
		public Guid? Key { get; }
		public IReadOnlyCollection<ClientParameter> Parameters => _params;

		private readonly List<ClientParameter> _params;

		public ClientMessage(string controller, string command, Guid? key, IEnumerable<ClientParameter> @params)
		{
			Command = command;
			Key = key;
			Controller = controller;
			_params = new List<ClientParameter>(@params);
		}

		public ClientMessage(string command, string controller, Guid? key = null)
			:this(command, controller, key,new ClientParameter[0])
		{
			
		}
	}
}
