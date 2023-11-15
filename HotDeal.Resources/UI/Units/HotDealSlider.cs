using System.Windows;
using System.Windows.Controls;

namespace HotDeal.Resources.UI.Units
{
	public class HotDealSlider : Slider
	{
		static HotDealSlider()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(HotDealSlider),
				new FrameworkPropertyMetadata(typeof(HotDealSlider)));
		}
	}
}
