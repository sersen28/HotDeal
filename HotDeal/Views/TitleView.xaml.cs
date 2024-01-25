using HotDeal.Items;
using HotDeal.Resources.Constants;
using System.Windows;
using System.Windows.Controls;

namespace HotDeal.Views
{
	/// <summary>
	/// Interaction logic for TitleView
	/// </summary>
	/// 

	public partial class TitleView : UserControl
	{
		public TitleViewType Type
		{
			get { return (TitleViewType)GetValue(TypeProperty); }
			set { SetValue(TypeProperty, value); }
		}

		public static readonly DependencyProperty TypeProperty =
			DependencyProperty.Register("Type", typeof(TitleViewType),
				typeof(TitleView));


		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string),
				typeof(TitleView));


		public TitleView()
		{
			InitializeComponent();
		}
    }
}
