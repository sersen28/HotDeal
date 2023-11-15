using System.Windows;
using System.Windows.Controls;

namespace HotDeal.Resources.UI.Units
{
	public class ListScrollViewer : ScrollViewer
	{
		static ListScrollViewer()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ListScrollViewer),
				new FrameworkPropertyMetadata(typeof(ListScrollViewer)));
		}
	}
}
