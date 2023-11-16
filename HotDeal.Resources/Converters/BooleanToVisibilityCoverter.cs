using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HotDeal.Resources.Converters
{
	public class BooleanToVisibilityCoverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool ret)
			{
				return ret ? Visibility.Visible : Visibility.Collapsed;
			}
			else
			{

				return Visibility.Collapsed;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
