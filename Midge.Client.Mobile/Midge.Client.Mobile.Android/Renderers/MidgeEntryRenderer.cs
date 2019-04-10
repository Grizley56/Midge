using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Midge.Client.Mobile.MidgeControls;
using Midge.Client.Mobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MidgeEntry), typeof(MidgeEntryRenderer))]
namespace Midge.Client.Mobile.Droid.Renderers
{
	public class MidgeEntryRenderer : EntryRenderer
	{
		public MidgeEntryRenderer(Context context)
			:base(context)
		{
			
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
				return;

			var midgeEntry = (MidgeEntry) Element;
			var underlineColor = midgeEntry.UnderlineColor.ToAndroid();
			var borderColor = midgeEntry.BorderColor.ToAndroid();

			GradientDrawable gd = new GradientDrawable();

			//Below line is useful to give border color
			gd.SetColor(borderColor);

			Control.SetHintTextColor(ColorStateList.ValueOf(underlineColor));

			//set underline color
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
				Control.BackgroundTintList = ColorStateList.ValueOf(underlineColor);
			else
				Control.Background.SetColorFilter(underlineColor, PorterDuff.Mode.SrcAtop);
		}
	}
}