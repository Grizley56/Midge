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
		public static MainActivity Instance { get; private set; }

		protected override void OnCreate(Bundle bundle)
		{
			Instance = this;
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;
			
			base.OnCreate(bundle);

			Forms.Init(this, bundle);
			Acr.UserDialogs.UserDialogs.Init(this);
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

