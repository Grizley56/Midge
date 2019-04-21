using System;
using System.Collections.Generic;

namespace Midge.Server.Communication.Core
{
	public class MidgeControllerInfo
	{
		public string ControllerName { get; }
		public Type ControllerType { get; }

		public IReadOnlyCollection<MidgeCommandInfo> ControllerCommands => _commands.Values;

		private readonly Dictionary<string, MidgeCommandInfo> _commands;

		public MidgeControllerInfo(string controllerName, Type controllerType, IEnumerable<MidgeCommandInfo> commands)
		{
			ControllerName = controllerName;
			ControllerType = controllerType;
			_commands = new Dictionary<string, MidgeCommandInfo>(StringComparer.OrdinalIgnoreCase);

			foreach (var command in commands)
				_commands.Add(command.CommandName, command);
		}

		public MidgeCommandInfo GetCommandByName(string name)
		{
			if (_commands.ContainsKey(name))
				return _commands[name];

			return null;
		}
	}
}
