using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsInput;
using WindowsInput.Native;
using Midge.Server.Services;
using Midge.Server.Windows.Utils;
using MouseButton = Midge.Server.Services.MouseButton;


namespace Midge.Server.Windows.Services
{
	public class ControlService: IControlService
	{
		private InputSimulator _input;

		public ControlService()
		{
			_input = new InputSimulator();
		}


		public (int x, int y) GetCursorPosition()
		{
			return Win32.GetCursorPosition();
		}

		public void SetCursorPosition(int x, int y)
		{
			Win32.SetCursorPosition(x, y);
		}

		public void MouseButtonClick(MouseButton button)
		{
			if (button == MouseButton.Left)
				Win32.MouseEvent(Win32.MouseEventFlags.LeftDown | Win32.MouseEventFlags.LeftUp);
			else if (button == MouseButton.Right)
				Win32.MouseEvent(Win32.MouseEventFlags.RightDown | Win32.MouseEventFlags.RightUp);
			else if (button == MouseButton.Middle)
				Win32.MouseEvent(Win32.MouseEventFlags.MiddleDown | Win32.MouseEventFlags.MiddleUp);
		}

		public void SendKey(int virtualKeyCode)
		{
			_input.Keyboard.KeyDown((VirtualKeyCode) virtualKeyCode);
			_input.Keyboard.KeyUp((VirtualKeyCode) virtualKeyCode);
		}

		public void KeyDown(int virtualKeyCode)
		{
			_input.Keyboard.KeyDown((VirtualKeyCode)virtualKeyCode);
		}

		public void KeyUp(int virtualKeyCode)
		{
			_input.Keyboard.KeyUp((VirtualKeyCode)virtualKeyCode);
		}
	}
}
