using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Midge.Client.Mobile.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidTimePicker))]
namespace Midge.Client.Mobile.Droid
{
	public class AndroidTimePicker: ITimePicker
	{

		public TimeSpan PickTime()
		{
			throw new Exception();
		}
	}

	class TimePickerFragment : DialogFragment
	{
		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			return new TimePickerDialog(Activity, (sender, args) =>
			{
				//args contains new time
			}, DateTime.Now.Hour, DateTime.Now.Minute, true);
		}
	}
}