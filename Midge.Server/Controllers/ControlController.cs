using System;
using System.Collections.Generic;
using System.Text;
using Midge.Server.Core;
using Midge.Server.Services;

namespace Midge.Server.Controllers
{
	[MidgeController("control")]
	class ControlController: ControllerBase
	{
		private readonly IControlService _controlService;
		public ControlController(IContext context, IServiceManager serviceManager) : base(context, serviceManager)
		{
			_controlService = Services.GetService<IControlService>();
		}

		[MidgeCommand("offsetMouse")]
		public void OffsetMouse(
			[MidgeParameter("offset_x", false)] float? x = null,
			[MidgeParameter("offset_y", false)] float? y = null)
		{
			if (x == null && y == null)
				return;

			(float mouseX, float mouseY) = _controlService.GetCursorPosition();

			if (x != null)
				mouseX += x.Value;
			if (y != null)
				mouseY += y.Value;

			_controlService.SetCursorPosition((int)mouseX, (int)mouseY);
		}

		[MidgeCommand("mouseClick")]
		public void MouseButtonClick([MidgeParameter("button")] int value)
		{
			MouseButton button = (MouseButton)value;

			_controlService.MouseButtonClick(button);
		}

		[MidgeCommand("keyUp")]
		public void KeyUp([MidgeParameter("virtual_key_code")] int code)
		{
			_controlService.KeyUp(code);
		}

		[MidgeCommand("keyDown")]
		public void KeyDown([MidgeParameter("virtual_key_code")] int code)
		{
			_controlService.KeyDown(code);
		}

		[MidgeCommand("sendKey")]
		public void SendKey([MidgeParameter("virtual_key_code")] int code)
		{
			_controlService.SendKey(code);
		}

	}
}
