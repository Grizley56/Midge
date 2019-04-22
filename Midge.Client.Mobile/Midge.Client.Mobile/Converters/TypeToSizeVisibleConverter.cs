using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Midge.Client.Mobile.ViewModel;
using Xamarin.Forms;

namespace Midge.Client.Mobile.Converters
{
	public class TypeToSizeVisibleConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (FileType) value == FileType.File;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
