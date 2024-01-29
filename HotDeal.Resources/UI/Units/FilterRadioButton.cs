using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HotDeal.Resources.UI.Units
{
	public class FilterRadioButton : RadioButton
	{
		static FilterRadioButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterRadioButton),
				new FrameworkPropertyMetadata(typeof(FilterRadioButton)));
		}

		public static readonly DependencyProperty NormalSourceProperty
			= DependencyProperty.Register("NormalSource", typeof(ImageSource)
				, typeof(FilterRadioButton));

		public ImageSource NormalSource
		{
			get => (ImageSource)GetValue(NormalSourceProperty);
			set => SetValue(NormalSourceProperty, value);
		}

		public static readonly DependencyProperty CheckedSourceProperty
			= DependencyProperty.Register("CheckedSource", typeof(ImageSource)
				, typeof(FilterRadioButton));

		public ImageSource CheckedSource
		{
			get => (ImageSource)GetValue(CheckedSourceProperty);
			set => SetValue(CheckedSourceProperty, value);
		}
	}
}
