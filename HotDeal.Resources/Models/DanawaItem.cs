using Reactive.Bindings;

namespace HotDeal.Resources.Models
{
	public class DanawaItem
	{
		public ReactiveProperty<string?> Description { get; set; } = new ();
		public ReactiveProperty<string?> Price { get; set; } = new();
		public ReactiveProperty<string?> Discount { get; set; } = new();
		public ReactiveProperty<string?> ImgSource { get; set; } = new();

		public DanawaItem(string description, string price, string discount, string imgSource)
		{
			this.Description.Value = description;
			this.Price.Value = price;
			this.Discount.Value = discount;
			this.ImgSource.Value = imgSource;
		}
	}
}
