using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Midge.Client.Mobile.MidgeControls
{
	public class MidgeFrame : Frame
	{
		public static BindableProperty ElevationProperty = BindableProperty.Create(nameof(Elevation), typeof(float), typeof(MidgeFrame), 12.0f);

		public float Elevation
		{
			get => (float)GetValue(ElevationProperty);
			set => SetValue(ElevationProperty, value);
		}
	}
}
