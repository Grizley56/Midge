using Newtonsoft.Json.Linq;

namespace Midge.Server.Communication
{
	public class ClientParameter
	{
		public string Name { get; }
		public JToken Value { get; }

		public ClientParameter(string name, JToken value)
		{
			Name = name;
			Value = value;
		}
	}
}
