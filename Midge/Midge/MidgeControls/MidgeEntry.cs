using Xamarin.Forms;

namespace Midge.MidgeControls
{
	public class MidgeEntry : Entry
	{
		public static readonly BindableProperty UnderlineColorProperty =
			BindableProperty.Create(nameof(UnderlineColor), typeof(Color), typeof(MidgeEntry), default(Color));

		public Color UnderlineColor
		{
			get => (Color) GetValue(UnderlineColorProperty);
			set => SetValue(UnderlineColorProperty, value);
		}

		public static readonly BindableProperty BorderColorProperty =
			BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(MidgeEntry), default(Color));

		public Color BorderColor
		{
			get => (Color) GetValue(BorderColorProperty);
			set => SetValue(BorderColorProperty, value);
		}
	}
}