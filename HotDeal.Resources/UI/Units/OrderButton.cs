using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotDeal.Resources.UI.Units
{
	public class OrderButton : RadioButton
	{
		public static readonly DependencyProperty IsAscendingProperty
			= DependencyProperty.Register("IsAscending", typeof(bool)
				, typeof(OrderButton));

		public bool IsAscending
		{
			get => (bool)GetValue(IsAscendingProperty);
			set => SetValue(IsAscendingProperty, value);
		}

		static OrderButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(OrderButton),
				new FrameworkPropertyMetadata(typeof(OrderButton)));
		}
	}
}
