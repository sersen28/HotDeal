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
	public class TMonViewModel : BindableBase
	{
		private readonly WebCrawlingService _webCrawlingService;
		private readonly LayoutService _layoutService;

		public ReadOnlyReactiveCollection<TMonModel> DanawaList { get; set; }
		public ReadOnlyReactiveCollection<TMonModel> DanawaFilterList { get; set; }

		public ReadOnlyReactivePropertySlim<bool> IsLoading { get; set; }
		public ReadOnlyReactivePropertySlim<LoadingSequence> LoadingSequence { get; set; }

		public ReactiveCommand<string> HyperlinkCommand { get; set; } = new();
		public ReactiveCommand<TMonModel> AddWishlistCommand { get; set; } = new();

		public TMonViewModel(WebCrawlingService webCrawlingService, LayoutService layoutService)
		{
			this._webCrawlingService = webCrawlingService;
			this._layoutService = layoutService;

			this.IsLoading = _webCrawlingService.IsTMonLoading.ToReadOnlyReactivePropertySlim();
			this.DanawaFilterList = _webCrawlingService.TmonFilterItems.ToReadOnlyReactiveCollection();
			this.LoadingSequence = _webCrawlingService.TMonLoadingSequence.ToReadOnlyReactivePropertySlim();

			this.HyperlinkCommand.Subscribe(this._webCrawlingService.OpenHyperlink);
			this.AddWishlistCommand.Subscribe(model => {
				this._webCrawlingService.AddItem(model);
			});
		}
	}
}
