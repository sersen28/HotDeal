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
