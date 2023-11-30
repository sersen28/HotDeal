using HotDeal.Resources.Models;
using HotDeal.Services;
using Prism.Mvvm;
using Reactive.Bindings;
using System;

namespace HotDeal.ViewModels
{
	public class DanawaViewModel : BindableBase
	{
		private readonly WebCrawlingService _webCrawlingService;
		private readonly LayoutService _layoutService;
		private readonly WishlistService _wishlistService;

		public ReadOnlyReactiveCollection<TMonModel> DanawaList { get; set; }
		public ReadOnlyReactiveCollection<TMonModel> DanawaFilterList { get; set; }

		public ReadOnlyReactivePropertySlim<bool> IsShowPopup { get; set; }
		public ReadOnlyReactivePropertySlim<bool> IsLoading { get; set; }
		public ReadOnlyReactivePropertySlim<LoadingSequence> LoadingSequence { get; set; }

		public ReactiveCommand<string> HyperlinkCommand { get; set; } = new();
		public ReactiveCommand<TMonModel> AddWishlistCommand { get; set; } = new();

		public DanawaViewModel(WebCrawlingService webCrawlingService, LayoutService layoutService, WishlistService wishlistService)
		{
			this._webCrawlingService = webCrawlingService;
			this._layoutService = layoutService;
			this._wishlistService = wishlistService;

			this.LoadingSequence = _webCrawlingService.DanawaLoadingSequence.ToReadOnlyReactivePropertySlim();
			this.IsShowPopup = this._layoutService.IsShowFilterPopup.ToReadOnlyReactivePropertySlim();
			this.IsLoading = _webCrawlingService.IsDanawaLoading.ToReadOnlyReactivePropertySlim();
			this.DanawaList = _webCrawlingService.DanawaItems.ToReadOnlyReactiveCollection();
			this.DanawaFilterList = _webCrawlingService.DanawaFilterItems.ToReadOnlyReactiveCollection();

			this.HyperlinkCommand.Subscribe(this._webCrawlingService.OpenHyperlink);
			this.AddWishlistCommand.Subscribe(model => {
				this._wishlistService.AddItem(WishlistModel.Convert(model));
			});
		}
	}
}
