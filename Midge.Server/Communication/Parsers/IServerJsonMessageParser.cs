using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Midge.Server.Communication.Parsers
{
	public interface IServerJsonMessageParser
	{
		JObject Parse(JToken body, Guid? key);
		JObject ParseError(string message, Guid? key);
	}
}
