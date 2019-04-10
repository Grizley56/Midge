using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using System.Text;
using Midge.Client.Mobile.Core;
using Xamarin.Forms;

namespace Midge.Client.Mobile.Converters
{
	public class StateToImageConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ConnectionState state = (ConnectionState)value;
			if (state == ConnectionState.Connected)
				return "Connected96.png";

			return "Disconnected96.png";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
