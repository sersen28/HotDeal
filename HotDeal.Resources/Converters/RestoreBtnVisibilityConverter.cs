using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System;

namespace HotDeal.Resources.Converters
{
	public class RestoreBtnVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is WindowState state)
			{
				return state == WindowState.Maximized ? Visibility.Visible : Visibility.Collapsed;
			}
			else
			{

				return Visibility.Collapsed;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
