using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Midge.Client.Mobile.View;
using Xamarin.Forms;

namespace Midge.Client.Mobile.Droid
{
	[Activity(Label = "Midge", Icon = "@mipmap/icon", Theme = "@style/AppTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public partial class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			Forms.Init(this, bundle);
			
			var app = new App();

			LoadApplication(app);

			SubscribeToKeyboardControlPage();
		}

		private void SubscribeToKeyboardControlPage()
		{
			MessagingCenter.Subscribe<KeyboardControlPage>(this, "forceLandscapeLayout",
				sender => { RequestedOrientation = ScreenOrientation.Landscape; });
			MessagingCenter.Subscribe<KeyboardControlPage>(this, "preventLandscapeLayout",
				sender => { RequestedOrientation = ScreenOrientation.Unspecified; });
		}


	}
}

