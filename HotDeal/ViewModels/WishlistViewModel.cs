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
		private readonly WebCrawlingService _webCrawlingService;

		public ReadOnlyReactiveCollection<TMonModel> Wishlist { get; set; }

		public ReactiveCommand<TMonModel> DeleteCommand { get; set; } = new();

		public WishlistViewModel(WebCrawlingService webCrawlingService)
        {
			this._webCrawlingService = webCrawlingService;

			this.Wishlist = webCrawlingService.Wishlist.ToReadOnlyReactiveCollection();

			this.DeleteCommand.Subscribe(this._webCrawlingService.DeleteItem);
		}
	}
}
