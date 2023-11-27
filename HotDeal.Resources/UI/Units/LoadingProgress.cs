using HotDeal.Resources.Models;
using System.Windows;
using System.Windows.Controls;

namespace HotDeal.Resources.UI.Units
{
	public class LoadingProgress : ProgressBar
	{
		public static readonly DependencyProperty StatusProperty
			= DependencyProperty.Register("Status", typeof(string)
				, typeof(string));

		public string Status
		{
			get => (string)GetValue(StatusProperty);
			set => SetValue(StatusProperty, value);
		}

		static LoadingProgress()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(LoadingProgress),
				new FrameworkPropertyMetadata(typeof(LoadingProgress)));
		}
	}
}
