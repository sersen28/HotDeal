using HotDeal.Resources.Models;
using HotDeal.Services;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotDeal.ViewModels
{
	public class GMarketViewModel : BindableBase
	{
		private readonly WebCrawlingService _webCrawlingService;
		private readonly LayoutService _layoutService;

		public ReadOnlyReactiveCollection<TMonModel> DanawaFilterList { get; set; }

		public ReadOnlyReactivePropertySlim<bool> IsLoading { get; set; }
		public ReadOnlyReactivePropertySlim<LoadingSequence> LoadingSequence { get; set; }

		public ReactiveCommand<string> HyperlinkCommand { get; set; } = new();
		public ReactiveCommand<TMonModel> AddWishlistCommand { get; set; } = new();

		public GMarketViewModel(WebCrawlingService webCrawlingService, LayoutService layoutService)
		{
			this._layoutService = layoutService;
			this._webCrawlingService = webCrawlingService;

			this.LoadingSequence = _webCrawlingService.GmarketLoadingSequence.ToReadOnlyReactivePropertySlim();
			this.IsLoading = _webCrawlingService.IsGMarketLoading.ToReadOnlyReactivePropertySlim();
			this.DanawaFilterList = _webCrawlingService.GMarketFilterItems.ToReadOnlyReactiveCollection();

			this.HyperlinkCommand.Subscribe(this._webCrawlingService.OpenHyperlink);
			this.AddWishlistCommand.Subscribe(model => {
				this._webCrawlingService.AddItem(model);
			});
		}
	}
}
