using System;
using Midge.Client.Mobile.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Midge.Client.Mobile
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new View.MainPage();
		}

		protected override async void OnStart()
		{
			MidgeCore.Instance.ReconnectionTimeout = 5000;
			await MidgeCore.Instance.Start();
			
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
