﻿using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace HotDeal.Resources.Models
{
	public class TMonModel
	{
		public ReactiveProperty<string?> Description { get; set; } = new();
		public ReactiveProperty<uint?> Discount { get; set; } = new();
		public ReactiveProperty<ulong?> Reduce { get; set; } = new();
		public ReactiveProperty<BitmapImage?> ImgSource { get; set; } = new();
		public ReactiveProperty<string?> Hyperlink { get; set; } = new();

		public ReactiveProperty<ulong?> Price { get; set; } = new();
		public ReactiveProperty<ulong?> OriginalPrice { get; set; } = new();

		public TMonModel() { }

		public TMonModel(string description, ulong price, uint discount, string imgSource, string hyperlink)
		{
			this.Description.Value = description;
			this.Price.Value = price;
			this.Discount.Value = discount;
			Application.Current.Dispatcher.BeginInvoke(() => {
				this.ImgSource.Value = new BitmapImage(new Uri(imgSource));
			});
			this.Hyperlink.Value = hyperlink;
			this.OriginalPrice.Value = this.Price.Value * 100 / (100 - this.Discount.Value);
			this.OriginalPrice.Value = this.OriginalPrice.Value - this.OriginalPrice.Value % 10;
			this.Reduce.Value = this.OriginalPrice.Value - this.Price.Value;
		}

		public TMonModel(string description, ulong price, ulong original, uint discount, string imgSource, string hyperlink)
		{
			this.Description.Value = description;
			this.Price.Value = price;
			this.Discount.Value = discount;
			Application.Current.Dispatcher.BeginInvoke(() => {
				this.ImgSource.Value = new BitmapImage(new Uri(imgSource));
			});
			this.Hyperlink.Value = hyperlink;
			this.OriginalPrice.Value = original;
			this.Reduce.Value = this.OriginalPrice.Value - this.Price.Value;
		}

		public TMonModel Convert(WishlistModel source)
		{
			var ret = new TMonModel { };
			this.Description.Value = source.Description.Value;
			this.Discount.Value = source.Discount.Value;
			this.Reduce.Value = source.Reduce.Value;
			this.ImgSource.Value = source.ImgSource.Value;
			this.Discount.Value = source.Discount.Value;
			this.Hyperlink.Value = source.Hyperlink.Value;
			this.Price.Value = source.Price.Value;
			this.OriginalPrice.Value = source.OriginalPrice.Value;

			return ret;
		}
	}
}
