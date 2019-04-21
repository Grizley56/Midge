using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace Midge.Server.Communication.Parsers
{
	internal class ServerMessageParser: IServerJsonMessageParser
	{
		public JObject Parse([NotNull] JToken body, Guid? key)
		{
			var message = new JObject(
				new JProperty("response", body));

			if (key != null)
				message.Add(
					new JProperty("command_token", key.Value));

			return message;
		}

		public JObject ParseError([NotNull] string error, Guid? key)
		{
			var message = new JObject(
				new JProperty("error", error));

			if (key != null)
				message.Add(
					new JProperty("command_token", key.Value));

			return message;
		}
	}
}
