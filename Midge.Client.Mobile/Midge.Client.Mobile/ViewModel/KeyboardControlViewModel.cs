using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Midge.API.Categories;
using Midge.Client.Mobile.Core;

namespace Midge.Client.Mobile.ViewModel
{
	public class KeyboardControlViewModel
	{
		private ControlCategory _control;

		public KeyboardControlViewModel()
		{
			_control = MidgeCore.Instance.Client.Control;
		}

		public async Task KeyDown(int virtualKeyCode)
		{
			await _control.KeyDown(virtualKeyCode);
		}

		public async Task KeyUp(int virtualKeyCode)
		{
			await _control.KeyUp(virtualKeyCode);
		}

		public async Task SendKey(int virtualKeyCode)
		{
			await _control.SendKey(virtualKeyCode);
		}
	}
}
