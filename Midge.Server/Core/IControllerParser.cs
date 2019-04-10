using Midge.Server.Controllers;

namespace Midge.Server.Core
{
	public interface IControllerParser
	{
		MidgeControllerInfo GetController(string name);
		bool Supports(string name);
	}
}