using Reactive.Bindings;
using System.Windows.Media.Imaging;

namespace HotDeal.Resources.Models
{
	public class WishlistModel
	{
		public ReactiveProperty<string?> Description { get; set; } = new();
		public ReactiveProperty<uint?> Discount { get; set; } = new();
		public ReactiveProperty<ulong?> Reduce { get; set; } = new();
		public ReactiveProperty<BitmapImage?> ImgSource { get; set; } = new();
		public ReactiveProperty<string?> Hyperlink { get; set; } = new();

		public ReactiveProperty<ulong?> Price { get; set; } = new();
		public ReactiveProperty<ulong?> OriginalPrice { get; set; } = new();

		public static WishlistModel Convert(TMonModel source)
		{
			var ret = new WishlistModel();
			ret.Description.Value = source.Description.Value;
			ret.Discount.Value = source.Discount.Value;
			ret.Reduce.Value = source.Reduce.Value;
			ret.ImgSource.Value = source.ImgSource.Value;
			ret.Discount.Value = source.Discount.Value;
			ret.Hyperlink.Value = source.Hyperlink.Value;
			ret.Price.Value = source.Price.Value;
			ret.OriginalPrice.Value = source.OriginalPrice.Value;
			return ret;
		}
	}
}
