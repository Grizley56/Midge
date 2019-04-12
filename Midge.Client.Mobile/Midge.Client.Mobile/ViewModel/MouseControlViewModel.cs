using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Midge.Client.Mobile.Core;
using Xamarin.Forms;

namespace Midge.Client.Mobile.ViewModel
{
	public class MouseControlViewModel
	{
		
		public MouseControlViewModel()
		{
			
		}

		public async Task MoveMouseCursor(Point pos)
		{
			float x = (float)pos.X * Settings.MouseSensitivity;
			float y = (float) pos.Y * Settings.MouseSensitivity;

			await MidgeCore.Instance.Client.Control.OffsetMouse(x, y);
		}

		public async Task LeftButtonClick()
		{
			await MidgeCore.Instance.Client.Control.MouseClick(API.Models.MouseButton.Left);
		}
	}
}
