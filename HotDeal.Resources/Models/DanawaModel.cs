using Reactive.Bindings;

namespace HotDeal.Resources.Models
{
	public class DanawaModel
	{
		public ReactiveProperty<string?> Description { get; set; } = new ();
		public ReactiveProperty<ulong?> Price { get; set; } = new();
		public ReactiveProperty<uint?> Discount { get; set; } = new();
		public ReactiveProperty<string?> ImgSource { get; set; } = new();

		public DanawaModel(string description, ulong price, uint discount, string imgSource)
		{
			this.Description.Value = description;
			this.Price.Value = price;
			this.Discount.Value = discount;
			this.ImgSource.Value = imgSource;
		}
	}
}
