using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Midge.Server.Communication.Core;
using Midge.Server.Controllers;
using Midge.Server.Utils;

namespace Midge.Server.Communication.Parsers
{
	public class MidgeControllerParser : IControllerParser
	{
		private Dictionary<string, MidgeControllerInfo> _controllers;

		public MidgeControllerParser()
		{
			Initialize();
		}

		private void Initialize()
		{
			var types = AttributeHelper.GetClassesWithAttribute<MidgeControllerAttribute>();

			_controllers = new Dictionary<string, MidgeControllerInfo>(StringComparer.OrdinalIgnoreCase);

			foreach (var type in types)
			{
				List<MidgeCommandInfo> commandInfo = new List<MidgeCommandInfo>();

				MidgeControllerAttribute attr = type
					.GetCustomAttributes(typeof(MidgeControllerAttribute), true)
					.Cast<MidgeControllerAttribute>()
					.First();

				if (!type.IsSubclassOf(typeof(ControllerBase)))
					continue;

				if (_controllers.ContainsKey(attr.ControllerName))
					throw new Exception($"Controller with name {attr.ControllerName} already exists");

				var methods = type.GetMethods().Where(i => i.GetCustomAttributes<MidgeCommandAttribute>().Any());
				foreach (var method in methods)
				{
					var methodAttr = method.GetCustomAttribute<MidgeCommandAttribute>();

					if (methodAttr == null)
						continue;

					var parameters = method.GetParameters();
					var paramsWithAttr = parameters.Where(i => i.GetCustomAttribute<MidgeParameterAttribute>() != null);

					if (paramsWithAttr.Count() != parameters.Length)
						throw new Exception($"Invalid parameters {method.Name}");

					MidgeParameterInfo[] parametersInfo = new MidgeParameterInfo[parameters.Length];

					for (var i = 0; i < parameters.Length; i++)
					{
						var param = parameters[i];
						MidgeParameterAttribute paramAttr = param.GetCustomAttribute<MidgeParameterAttribute>();

						parametersInfo[i] = new MidgeParameterInfo(param.ParameterType, paramAttr.Name, paramAttr.IsRequired, i);
					}

					commandInfo.Add(new MidgeCommandInfo(methodAttr.CommandName, method, parametersInfo));
				}

				_controllers.Add(attr.ControllerName, new MidgeControllerInfo(attr.ControllerName, type, commandInfo));
			}
		}

		public MidgeControllerInfo GetController(string name)
		{
			if (_controllers.ContainsKey(name))
				return _controllers[name];

			return null;
		}

		public bool Supports(string name)
		{
			return _controllers.ContainsKey(name);
		}

	}
}
