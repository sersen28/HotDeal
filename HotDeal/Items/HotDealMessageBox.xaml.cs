using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotDeal.Items
{
    /// <summary>
    /// HotDealMessageBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HotDealMessageBox : Window
	{
		public enum DWMWINDOWATTRIBUTE
		{
			DWMWA_WINDOW_CORNER_PREFERENCE = 33
		}

		public enum DWM_WINDOW_CORNER_PREFERENCE
		{
			DWMWCP_DEFAULT = 0,
			DWMWCP_DONOTROUND = 1,
			DWMWCP_ROUND = 2,
			DWMWCP_ROUNDSMALL = 3
		}

		[DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
		internal static extern void DwmSetWindowAttribute(IntPtr hwnd,
														 DWMWINDOWATTRIBUTE attribute,
														 ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute,
														 uint cbAttribute);

		public string Message
		{
			get { return (string)GetValue(MessageProperty); }
			set { SetValue(MessageProperty, value); }
		}

		public static readonly DependencyProperty MessageProperty =
			DependencyProperty.Register("Message", typeof(string),
				typeof(HotDealMessageBox));

		public HotDealMessageBox()
        {
            InitializeComponent();

			IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();
			var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
			var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
			DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint));
		}

		private void Cancel(object sender, RoutedEventArgs e)
		{
			SystemCommands.CloseWindow(this);
		}
	}
}
