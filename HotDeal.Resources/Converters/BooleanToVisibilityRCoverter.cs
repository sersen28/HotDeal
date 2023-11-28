using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace HotDeal.Resources.Converters
{
	public class BooleanToVisibilityRCoverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool ret)
			{
				return ret ? Visibility.Collapsed : Visibility.Visible;
			}
			else
			{

				return Visibility.Visible;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
