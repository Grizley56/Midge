using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.Server.Windows
{
	public interface ILogger
	{
		void Log(string text);
		void Log(Exception ex);
	}
}
