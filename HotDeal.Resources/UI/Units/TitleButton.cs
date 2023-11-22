using System.Windows;
using System.Windows.Controls;

namespace HotDeal.Resources.UI.Units
{
	public class TitleButton : Button
	{
		static TitleButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleButton),
				new FrameworkPropertyMetadata(typeof(TitleButton)));
		}
	}
}
