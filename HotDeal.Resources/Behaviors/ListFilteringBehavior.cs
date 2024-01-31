using Microsoft.Xaml.Behaviors;
using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using HotDeal.Resources.Models;

namespace HotDeal.Resources.Behaviors
{
    public class ListFilteringBehavior : Behavior<ContentControl>
	{
		ContentControl? _view = null;

		public static readonly DependencyProperty MinPriceProperty =
			DependencyProperty.Register("MinPrice", typeof(ulong), typeof(ListFilteringBehavior),
			new FrameworkPropertyMetadata(OnPropertyChanged));

		public ulong MinPrice
		{
			get
			{
				return (ulong)GetValue(MinPriceProperty);
			}
			set
			{
				SetValue(MinPriceProperty, value);
			}
		}

		public static readonly DependencyProperty MaxPriceProperty =
			DependencyProperty.Register("MaxPrice", typeof(ulong), typeof(ListFilteringBehavior),
			new FrameworkPropertyMetadata(OnPropertyChanged));

		public ulong MaxPrice
		{
			get
			{
				return (ulong)GetValue(MaxPriceProperty);
			}
			set
			{
				SetValue(MaxPriceProperty, value);
			}
		}


		public static readonly DependencyProperty DiscountProperty =
			DependencyProperty.Register("Discount", typeof(uint), typeof(ListFilteringBehavior),
			new FrameworkPropertyMetadata(OnPropertyChanged));

		public uint Discount
		{
			get
			{
				return (uint)GetValue(DiscountProperty);
			}
			set
			{
				SetValue(DiscountProperty, value);
			}
		}


		public static readonly DependencyProperty DataProperty =
			DependencyProperty.Register("Data", typeof(TMonModel), typeof(ListFilteringBehavior),
			new FrameworkPropertyMetadata(OnPropertyChanged));

		public TMonModel Data
		{
			get
			{
				return (TMonModel)GetValue(DataProperty);
			}
			set
			{
				SetValue(DataProperty, value);
			}
		}


		public static readonly DependencyProperty AllowFilterProperty =
			DependencyProperty.Register("AllowFilter", typeof(bool), typeof(ListFilteringBehavior),
			new FrameworkPropertyMetadata(OnPropertyChanged));

		public bool AllowFilter
		{
			get
			{
				return (bool)base.GetValue(AllowFilterProperty);
			}
			set
			{
				base.SetValue(AllowFilterProperty, value);
			}
		}

		private static void OnFilterChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
		{
			var obj = dobj as ListFilteringBehavior;
			var view = obj?.AssociatedObject;

			if (obj?.AssociatedObject is not null)
			{
				obj.SetItemVisibility();
			}
		}

		private static void OnPropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
		{
			var obj = dobj as ListFilteringBehavior;
			var view = obj?.AssociatedObject;

			if (obj?.AssociatedObject is not null)
			{
				obj.SetItemVisibility();
			}
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			var view = this.AssociatedObject;
			if (view != null)
			{
				_view = view;
			}

			SetItemVisibility();
		}

		private void SetItemVisibility()
		{
			if (this._view is null) return;

			var allow = (bool)GetValue(AllowFilterProperty);
			if (allow is not true)
			{
				this._view.Visibility = Visibility.Visible;
				return;
			}

			var minPrice = (ulong)GetValue(MinPriceProperty);
			var maxPrice = (ulong)GetValue(MaxPriceProperty);
			var discount = (uint)GetValue(DiscountProperty);
			var data = (TMonModel)GetValue(DataProperty);

			if (data is null) return;

			if (minPrice > data.Price.Value
				|| maxPrice < data.Price.Value
				|| discount > data.Discount.Value)
			{
				this._view.Visibility = Visibility.Collapsed;
				return;
			}

			this._view.Visibility = Visibility.Visible;
		}
	}
}
