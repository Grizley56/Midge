using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Midge.Server.Windows.Utils
{
	public static class Win32
	{
		public enum CtrlType
		{
			CtrlCEvent = 0,
			CtrlBreakEvent = 1,
			CtrlCloseEvent = 2,
			CtrlLogoffEvent = 5,
			CtrlShutdownEvent = 6
		}

		[Flags]
		public enum MouseEventFlags
		{
			LeftDown = 0x00000002,
			LeftUp = 0x00000004,
			MiddleDown = 0x00000020,
			MiddleUp = 0x00000040,
			Move = 0x00000001,
			Absolute = 0x00008000,
			RightDown = 0x00000008,
			RightUp = 0x00000010
		}


		public delegate bool ConsoleCtrlEventHandler(CtrlType ctrlType);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool SetConsoleCtrlHandler(ConsoleCtrlEventHandler handler, bool add);

		[DllImport("kernel32", SetLastError = true)]
		public static extern bool GenerateConsoleCtrlEvent(CtrlType ctrlType, int dwProcessGroupId);

		[DllImport("kernel32", SetLastError = true)]
		public static extern bool AllocConsole();

		[DllImport("kernel32", SetLastError = true)]
		public static extern bool FreeConsole();

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool AttachConsole(uint dwProcessId);

		[DllImport("user32.dll")]
		private static extern bool SetCursorPos(int X, int Y);

		[DllImport("user32.dll")]
		private static extern bool GetCursorPos(ref Win32Point point);

		[DllImport("user32.dll")]
		private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

		
		private struct Win32Point
		{
			public int X;
			public int Y;
		}


		public static void SetCursorPosition(int x, int y)
		{
			SetCursorPos(x, y);
		}

		public static (int x, int y) GetCursorPosition()
		{
			Win32Point p = new Win32Point();
			GetCursorPos(ref p);
			return (p.X, p.Y);
		}

		public static void MouseEvent(MouseEventFlags value)
		{
			var (x, y) = GetCursorPosition();
			mouse_event((int)value,x, y, 0, 0);
		}
	}
}
