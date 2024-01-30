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

		public ReadOnlyReactiveCollection<TMonModel> DanawaFilterList { get; set; }

		public ReadOnlyReactivePropertySlim<bool> IsLoading { get; set; }
		public ReadOnlyReactivePropertySlim<LoadingSequence> LoadingSequence { get; set; }

		public ReactiveCommand<string> HyperlinkCommand { get; set; } = new();
		public ReactiveCommand<TMonModel> AddWishlistCommand { get; set; } = new();

		public DanawaViewModel(WebCrawlingService webCrawlingService, LayoutService layoutService)
		{
			this._webCrawlingService = webCrawlingService;
			this._layoutService = layoutService;

			this.LoadingSequence = _webCrawlingService.DanawaLoadingSequence.ToReadOnlyReactivePropertySlim();
			this.IsLoading = _webCrawlingService.IsDanawaLoading.ToReadOnlyReactivePropertySlim();
			this.DanawaFilterList = _webCrawlingService.DanawaFilterItems.ToReadOnlyReactiveCollection();

			this.HyperlinkCommand.Subscribe(this._webCrawlingService.OpenHyperlink);
			this.AddWishlistCommand.Subscribe(model => {
				if (model.IsAddedWishList.Value)
				{
					this._webCrawlingService.DeleteItem(model);
				}
				else
				{
					this._webCrawlingService.AddItem(model);
				}
			});
		}
	}
}
