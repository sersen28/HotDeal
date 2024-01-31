using HotDeal.Resources.Constants;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

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


		public MessageBoxType Type
		{
			get { return (MessageBoxType)GetValue(TypeProperty); }
			set { SetValue(TypeProperty, value); }
		}
		public static readonly DependencyProperty TypeProperty =
			DependencyProperty.Register("Type", typeof(MessageBoxType),
				typeof(HotDealMessageBox));


		public string MessageTitle
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public static readonly DependencyProperty MessageTitleProperty =
			DependencyProperty.Register("MessageTitle", typeof(string),
				typeof(HotDealMessageBox));

		public HotDealMessageBox()
        {
            InitializeComponent();

			IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();
			var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
			var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
			DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint));
		}

		private void OK(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			SystemCommands.CloseWindow(this);
		}

		private void Cancel(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			SystemCommands.CloseWindow(this);
		}
	}
}
