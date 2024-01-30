using HotDeal.Resources.Models;
using HotDeal.Services;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Reactive.Linq;

namespace HotDeal.ViewModels
{
	public class WishlistViewModel : BindableBase
	{
		private readonly WishlistService _wishlistService;
		private readonly WebCrawlingService _webCrawlingService;

		public ReadOnlyReactiveCollection<WishlistModel> Wishlist { get; set; }
		public ReadOnlyReactiveCollection<TMonModel> test { get; set; }

		public ReactiveCommand<WishlistModel> DeleteCommand { get; set; } = new();

		public WishlistViewModel(WishlistService wishlistService, WebCrawlingService webCrawlingService)
        {
			this._wishlistService = wishlistService;
			this._webCrawlingService = webCrawlingService;

			this.Wishlist = _wishlistService.Wishlist.ToReadOnlyReactiveCollection();

			this.DeleteCommand.Subscribe(this._wishlistService.DeleteItem);
		}
	}
}
