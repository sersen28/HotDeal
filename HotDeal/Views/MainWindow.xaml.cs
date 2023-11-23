using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace HotDeal.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}
