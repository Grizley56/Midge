using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Midge.Server.Communication.Parsers
{
	internal class ClientMessageParser : IClientJsonMessageParser
	{
		public ClientMessageParser()
		{
			
		}


		public ClientMessage ParseFromJson(JToken json)
		{
			var controllerMethod = json.SelectToken("method", false)?.Value<string>();

			Guid? commandToken = null;

			var k = json.SelectToken("command_token");

			if (k != null)
			{
				try
				{
					commandToken = k.ToObject<Guid>();
				}
				catch
				{
					throw new ClientMessageParserException("invalid command_token format");
				}
			}
			
			
			if (controllerMethod == null || string.IsNullOrWhiteSpace(controllerMethod))
				throw new ClientMessageParserException("unknown method");

			var split = controllerMethod.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

			string command = null;

			if (split.Length > 2)
				throw new ClientMessageParserException("invalid method format");

			var controller = split[0];

			if (split.Length > 0)
				command = split[1];

			var parameters = json.SelectToken("params", false)?.Children<JProperty>();

			if (parameters == null)
				return new ClientMessage(controller, command, commandToken);
			else
				return new ClientMessage(controller, command, commandToken, ParseParams(parameters));
		}

		public ClientMessage ParseFromJson(string json)
		{
			JToken token;

			try
			{
				token = JToken.Parse(json);
			}
			catch(JsonException)
			{
				throw new ClientMessageParserException("invalid json format");
			}

			return ParseFromJson(token);
		}

		private IEnumerable<ClientParameter> ParseParams(IJEnumerable<JProperty> props)
		{
			foreach (var prop in props)
				yield return new ClientParameter(prop.Name, prop);
		}
	}
}
