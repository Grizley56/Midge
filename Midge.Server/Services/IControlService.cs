using System;
using System.Collections.Generic;
using System.Text;

namespace Midge.Server.Services
{
	public interface IControlService
	{
		(int x, int y) GetCursorPosition();
		void SetCursorPosition(int x, int y);
		void MouseButtonClick(MouseButton button);

		void SendKey(int virtualKeyCode);
		void KeyDown(int virtualKeyCode);
		void KeyUp(int virtualKeyCode);
	}
}
