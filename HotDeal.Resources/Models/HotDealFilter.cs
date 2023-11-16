using Reactive.Bindings;
using System.Collections.Generic;

namespace HotDeal.Resources.Models
{
	public class HotDealFilter
	{
		public const ulong DefaultMinimumPrice = 0;
		public const ulong DefaultMaximumPrice = 300000;
		public const uint DefaultDiscount = 25;

		public List<string> BlockList { get; set; } = new();
		public ReactiveProperty<uint> Discount { get; set; } = new(DefaultDiscount);
		public ReactiveProperty<ulong> MinimumPrice { get; set; } = new(DefaultMinimumPrice);
		public ReactiveProperty<ulong> MaximumPrice { get; set; } = new(DefaultMaximumPrice);

		public HotDealFilter()
		{
		}
	}
}
