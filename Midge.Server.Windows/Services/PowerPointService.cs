using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.PowerPoint;
using Midge.Server.Services;

namespace Midge.Server.Windows.Services
{
	public class PowerPointService: IPresentationService
	{
		private readonly Application _powerPoint;

		public PowerPointService()
		{
			_powerPoint = new Application();
		}

		public void MoveNext()
		{
			try
			{
				if (IsLaunched)
					_powerPoint.ActivePresentation.SlideShowWindow.View.Next();
			}
			catch (System.Runtime.InteropServices.COMException)
			{
				return;
			}
		}

		public void MovePrevious()
		{
			try
			{
				if (IsLaunched)
					_powerPoint.ActivePresentation.SlideShowWindow.View.Previous();
			}
			catch (System.Runtime.InteropServices.COMException)
			{
				return;
			}
		}

		public void Play()
		{
			try
			{
				if (IsLaunched)
				{
					if (_powerPoint.SlideShowWindows.Count == 0)
						_powerPoint.ActivePresentation.SlideShowSettings.Run();
					else
						_powerPoint.ActivePresentation.SlideShowWindow.View.Exit();
				}
			}
			catch (System.Runtime.InteropServices.COMException)
			{
				return;
			}
		}

		public bool IsLaunched => _powerPoint.Presentations.Count != 0;
	}
}
