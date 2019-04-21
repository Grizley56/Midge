using Newtonsoft.Json.Linq;

namespace Midge.Server.Communication.Parsers
{
	internal interface IClientJsonMessageParser
	{
		ClientMessage ParseFromJson(JToken json);
		ClientMessage ParseFromJson(string json);
	}
}