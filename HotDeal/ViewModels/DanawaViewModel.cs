using HotDeal.Resources.Constants;
using HotDeal.Resources.Models;
using HotDeal.Services;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System;

namespace HotDeal.ViewModels
{
	public class DanawaViewModel : BindableBase
	{
		private readonly WebCrawlingService _webCrawlingService;
		private readonly LayoutService _layoutService;

		public ReadOnlyReactiveCollection<TMonModel> ItemsSource { get; set; }
		public ReadOnlyReactivePropertySlim<HotDealFilter> UserFilter { get; set; }
		public ReadOnlyReactivePropertySlim<LoadingSequence> LoadingSequence { get; set; }

		public ReactiveCommand<string> HyperlinkCommand { get; set; } = new();
		public ReactiveCommand<TMonModel> AddWishlistCommand { get; set; } = new();
		public ReadOnlyReactivePropertySlim<bool> AllowFilter { get; set; }

		public DanawaViewModel(WebCrawlingService webCrawlingService, LayoutService layoutService)
		{
			this._webCrawlingService = webCrawlingService;
			this._layoutService = layoutService;

			this.UserFilter = this._webCrawlingService.UserFilter.ToReadOnlyReactivePropertySlim();
			this.AllowFilter = this._webCrawlingService.AllowFilter.ToReadOnlyReactivePropertySlim();
			this.LoadingSequence = _webCrawlingService.DanawaLoadingSequence.ToReadOnlyReactivePropertySlim();
			this.ItemsSource = _webCrawlingService.DanawaDatas.ToReadOnlyReactiveCollection();

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
