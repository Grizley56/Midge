using System.ComponentModel;
using Android.Content;
using Android.Support.V4.View;
using Midge.Droid.Renderers;
using Midge.MidgeControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FrameRenderer = Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer;


[assembly: ExportRenderer(typeof(MidgeFrame), typeof(MidgeFrameRenderer))]
namespace Midge.Droid.Renderers
{
	class MidgeFrameRenderer: FrameRenderer
	{
		public MidgeFrameRenderer(Context context)
			: base(context)
		{

		}

		protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
		{
			base.OnElementChanged(e);
			if (e.NewElement == null)
				return;

			UpdateElevation();
		}


		private void UpdateElevation()
		{
			var materialFrame = (MidgeFrame)Element;

			// we need to reset the StateListAnimator to override the setting of Elevation on touch down and release.
			Control.StateListAnimator = new Android.Animation.StateListAnimator();

			ViewCompat.SetElevation(this, materialFrame.Elevation);
			ViewCompat.SetElevation(Control, materialFrame.Elevation);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == "Elevation")
			{
				UpdateElevation();
			}
		}
	}
}