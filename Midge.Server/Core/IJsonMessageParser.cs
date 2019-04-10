using Newtonsoft.Json.Linq;

namespace Midge.Server.Core
{
	public interface IJsonMessageParser
	{
		JsonMessage Parse(JToken json);
		JsonMessage Parse(string json);
	}
}