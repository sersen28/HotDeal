using HotDeal.Resources.Models;
using Reactive.Bindings;

namespace HotDeal.Services
{
	public class WishlistService
    {
        public ReactiveCollection<WishlistModel> Wishlist { get; set; } = new();

        public WishlistService() 
        {

		}

		public void AddItem(WishlistModel item)
		{
			Wishlist.AddOnScheduler(item);
		}

		public void DeleteItem(WishlistModel item)
        {
            Wishlist.RemoveOnScheduler(item);
        }
    }
}
