using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.ServiceModel;
using System.Text;
using Xamarin.Forms;

namespace Midge.Converters
{
	public class StateToImageConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			CommunicationState state = (CommunicationState)value;
			if (state == CommunicationState.Opened)
				return "Connected96.png";

			return "Disconnected96.png";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
