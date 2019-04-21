using Midge.Server.Communication.Core;

namespace Midge.Server.Communication.Parsers
{
	public interface IControllerParser
	{
		MidgeControllerInfo GetController(string name);
		bool Supports(string name);
	}
}