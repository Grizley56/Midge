using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midge.Server.Core
{
	public class JsonMessageParserException: ApplicationException
	{
		public JsonMessageParserException(string message)
			:base(message)
		{
			
		}

		public JsonMessageParserException(string message, Exception inner)
			:base(message, inner)
		{
			
		}
	}
}
