using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using JetBrains.Annotations;

namespace Midge.Server
{
	public class MidgeException: ApplicationException
	{
		public MidgeException()
		{

		}

		protected MidgeException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public MidgeException(string message) : base(message)
		{
		}

		public MidgeException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
