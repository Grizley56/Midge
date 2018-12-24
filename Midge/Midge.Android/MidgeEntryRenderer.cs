using System;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Midge;
using Midge.Droid;
using Midge.MidgeControls;

[assembly: ExportRenderer(typeof(MidgeEntry), typeof(CustomEntryRenderer))]
namespace Midge.Droid
{
	public class CustomEntryRenderer : EntryRenderer
	{
		public CustomEntryRenderer(Context context)
			:base(context)
		{
			
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
				return;

			GradientDrawable gd = new GradientDrawable();

			//Below line is useful to give border color
			gd.SetColor(Android.Graphics.Color.Red);

			Control.SetHintTextColor(ColorStateList.ValueOf(Android.Graphics.Color.White));

			//set underline color
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
				Control.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.White);
			else
				Control.Background.SetColorFilter(Android.Graphics.Color.White, PorterDuff.Mode.SrcAtop);
		}
	}
}