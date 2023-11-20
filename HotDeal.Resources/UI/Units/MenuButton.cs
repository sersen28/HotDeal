using System.Windows;
using System.Windows.Controls;

namespace HotDeal.Resources.UI.Units
{
	public class MenuButton : RadioButton
	{
		static MenuButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuButton),
				new FrameworkPropertyMetadata(typeof(MenuButton)));
		}
	}
}
