using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotDeal.Resources.Models
{
	public class TMonModel
	{
		public ReactiveProperty<string?> Description { get; set; } = new();
		public ReactiveProperty<uint?> Discount { get; set; } = new();
		public ReactiveProperty<string?> ImgSource { get; set; } = new();
		public ReactiveProperty<ulong?> Price { get; set; } = new();
		//public ReactiveProperty<ulong?> OriginalPrice { get; set; } = new();

		public TMonModel(string description, ulong price, uint discount, string imgSource)
		{
			this.Description.Value = description;
			this.Price.Value = price;
			this.Discount.Value = discount;
			this.ImgSource.Value = imgSource;
		}
	}
}
