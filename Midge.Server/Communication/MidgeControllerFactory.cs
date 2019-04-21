using System;
using System.Collections.Generic;
using System.Linq;
using Midge.Server.Communication.Core;
using Midge.Server.Communication.Parsers;

namespace Midge.Server.Communication
{
	public class MidgeControllerFactory
	{
		public IControllerParser ControllerParser { get; set; }

		public MidgeControllerFactory(IControllerParser controllerControllerParser)
		{
			ControllerParser = controllerControllerParser;
		}

		public MidgeCommandInvoker Create(ClientMessage clientMessage)
		{
			MidgeControllerInfo controllerInfo = ControllerParser.GetController(clientMessage.Controller);

			MidgeCommandInfo commandInfo = controllerInfo.GetCommandByName(clientMessage.Command);

			if (commandInfo == null)
				throw new MidgeException("Command undefined");

			List<object> args = new List<object>();

			foreach (var param in commandInfo.Params.OrderBy(i => i.Order))
			{
				var userParam = clientMessage.Parameters.FirstOrDefault(i =>
					i.Name.Equals(param.Name, StringComparison.OrdinalIgnoreCase));

				if (userParam == null && param.IsRequired)
					throw new MidgeException($"Parameter not found. Param name: {param.Name}");

				Type actualParamType = param.ParamType;

				if (userParam == null)
				{
					args.Add(null);
					continue;
				}

				if (param.ParamType.IsGenericType && param.ParamType.GetGenericTypeDefinition() == typeof(Nullable<>))
					actualParamType = param.ParamType.GetGenericArguments()[0];


				object argument;



				try
				{
					argument = userParam.Value.ToObject(actualParamType);

					if (actualParamType.IsEnum)
					{
						if (!Enum.IsDefined(actualParamType, argument))
							throw new Exception("Invalid value");

						argument = Enum.ToObject(actualParamType, (int) argument);
					}
				}
				catch
				{
					throw new MidgeException($"Parameter {param.Name} has invalid type. Expected type is {actualParamType.Name}");
				}

				args.Add(argument);
			}


			return new MidgeCommandInvoker(controllerInfo, commandInfo, args.ToArray());
		}
	}
}
