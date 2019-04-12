using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midge.API.Models;

namespace Midge.API.Categories
{
	public class ControlCategory: Category
	{

		public async Task OffsetMouse(float x, float y)
		{
			await MidgeInvoke.SendAsync("control.offsetMouse", new MidgeParameters
			{
				{"offset_x", x},
				{"offset_y", y}
			});
		}

		public async Task MouseClick(MouseButton button)
		{
			await MidgeInvoke.SendAsync("control.mouseClick", new MidgeParameters
			{
				{"button", (int) button}
			});
		}

		public async Task KeyDown(int virtualKeyCode)
		{
			await MidgeInvoke.SendAsync("control.keyDown", new MidgeParameters
			{
				{"virtual_key_code", virtualKeyCode}
			});
		}

		public async Task KeyUp(int virtualKeyCode)
		{
			await MidgeInvoke.SendAsync("control.keyUp", new MidgeParameters
			{
				{"virtual_key_code", virtualKeyCode}
			});
		}

		public async Task SendKey(int virtualKeyCode)
		{
			await MidgeInvoke.SendAsync("control.sendKey", new MidgeParameters
			{
				{"virtual_key_code", virtualKeyCode}
			});
		}

		public ControlCategory(IMidgeInvoke midgeInvoke) : base(midgeInvoke)
		{
		}
	}
}
