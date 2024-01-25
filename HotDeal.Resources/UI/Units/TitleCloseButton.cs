using System.Windows;
using System.Windows.Controls;

namespace HotDeal.Resources.UI.Units
{
    public class TitleCloseButton : Button
	{
		public Thickness ContentMargin
		{
			get { return (Thickness)GetValue(ContentMarginProperty); }
			set { SetValue(ContentMarginProperty, value); }
		}

		public static readonly DependencyProperty ContentMarginProperty =
			DependencyProperty.Register("ContentMargin", typeof(Thickness),
				typeof(TitleCloseButton));

		static TitleCloseButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleCloseButton),
				new FrameworkPropertyMetadata(typeof(TitleCloseButton)));
		}
	}
}
