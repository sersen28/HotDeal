using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotDeal.Resources.Models
{
	public class TMonModel
	{
		public ReactiveProperty<string?> Description { get; set; } = new();
		public ReactiveProperty<uint?> Discount { get; set; } = new();
		public ReactiveProperty<ulong?> Reduce { get; set; } = new();
		public ReactiveProperty<string?> ImgSource { get; set; } = new();

		public ReactiveProperty<ulong?> Price { get; set; } = new();
		public ReactiveProperty<ulong?> OriginalPrice { get; set; } = new();

		public TMonModel(string description, ulong price, uint discount, string imgSource)
		{
			this.Description.Value = description;
			this.Price.Value = price;
			this.Discount.Value = discount;
			this.ImgSource.Value = imgSource;
			this.OriginalPrice.Value = this.Price.Value * 100 / (100 - this.Discount.Value);
			this.OriginalPrice.Value = this.OriginalPrice.Value - this.OriginalPrice.Value % 10;
			this.Reduce.Value = this.OriginalPrice.Value - this.Price.Value;
		}

		public TMonModel(string description, ulong price, ulong original, uint discount, string imgSource)
		{
			this.Description.Value = description;
			this.Price.Value = price;
			this.Discount.Value = discount;
			this.ImgSource.Value = imgSource;
			this.OriginalPrice.Value = original;
			this.Reduce.Value = this.OriginalPrice.Value - this.Price.Value;
		}
	}
}
