using Android.Content;
using Midge.Droid.Renderers;
using Midge.MidgeControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MidgeSlider), typeof(MidgeSliderRenderer))]
namespace Midge.Droid.Renderers
{
	class MidgeSliderRenderer: SliderRenderer
	{
		public MidgeSliderRenderer(Context ctx)
			:base(ctx)
		{
			
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || e.NewElement == null)
				return;

			var view = (MidgeSlider)Element;
			

		}
	}
}