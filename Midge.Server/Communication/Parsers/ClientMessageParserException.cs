using System;

namespace Midge.Server.Communication.Parsers
{
	public class ClientMessageParserException: MidgeException
	{
		public ClientMessageParserException(string message)
			:base(message)
		{
			
		}

		public ClientMessageParserException(string message, Exception inner)
			:base(message, inner)
		{
			
		}
	}
}
