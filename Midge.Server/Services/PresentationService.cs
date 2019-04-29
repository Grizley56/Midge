using System;
using System.Collections.Generic;
using System.Text;

namespace Midge.Server.Services
{
	public interface IPresentationService
	{
		void MoveNext();
		void MovePrevious();
		void Play();

		bool IsLaunched { get; }
	}
}
