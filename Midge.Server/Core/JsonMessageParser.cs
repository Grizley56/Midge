using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Midge.Server.Core
{
	public class JsonMessageParser : IJsonMessageParser
	{
		public JsonMessageParser()
		{
			
		}


		public JsonMessage Parse(JToken json)
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
					throw new JsonMessageParserException("invalid command_token format");
				}
			}
			
			
			if (controllerMethod == null || string.IsNullOrWhiteSpace(controllerMethod))
				throw new JsonMessageParserException("unknown method");

			var split = controllerMethod.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

			string command = null;

			if (split.Length > 2)
				throw new JsonMessageParserException("invalid method format");

			var controller = split[0];

			if (split.Length > 0)
				command = split[1];

			var parameters = json.SelectToken("params", false)?.Children<JProperty>();

			if (parameters == null)
				return new JsonMessage(controller, command, commandToken);
			else
				return new JsonMessage(controller, command, commandToken, ParseParams(parameters));
		}

		public JsonMessage Parse(string json)
		{
			JToken token;

			try
			{
				token = JToken.Parse(json);
			}
			catch(JsonException)
			{
				throw new JsonMessageParserException("invalid json format");
			}

			return Parse(token);
		}

		private IEnumerable<Parameter> ParseParams(IJEnumerable<JProperty> props)
		{
			foreach (var prop in props)
				yield return new Parameter(prop.Name, prop);
		}
	}
}
