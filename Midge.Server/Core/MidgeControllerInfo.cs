using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Midge.Server.Core
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

	public class MidgeCommandInfo
	{
		public string CommandName { get; }
		public MethodInfo MethodInfo { get; }
		public IEnumerable<MidgeParameterInfo> Params { get; }
		public bool IsAsync { get; }

		public MidgeCommandInfo(string commandName, MethodInfo methodInfo, IEnumerable<MidgeParameterInfo> @params)
		{
			CommandName = commandName;
			MethodInfo = methodInfo;
			Params = @params;

			if (MethodInfo.ReturnType == typeof(Task))
				IsAsync = true;
		}
	}

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
