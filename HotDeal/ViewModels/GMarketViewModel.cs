using HotDeal.Resources.Models;
using HotDeal.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotDeal.ViewModels
{
	public class GMarketViewModel : BindableBase, INavigationAware
	{
		private readonly WebCrawlingService _webCrawlingService;
		private readonly LayoutService _layoutService;

		public ReadOnlyReactiveCollection<TMonModel> ItemsSource { get; set; }

		public ReadOnlyReactivePropertySlim<LoadingSequence> LoadingSequence { get; set; }

		public ReactiveCommand<string> HyperlinkCommand { get; set; } = new();
		public ReactiveCommand<TMonModel> AddWishlistCommand { get; set; } = new();

		public GMarketViewModel(WebCrawlingService webCrawlingService, LayoutService layoutService)
		{
			this._layoutService = layoutService;
			this._webCrawlingService = webCrawlingService;

			this.LoadingSequence = _webCrawlingService.GmarketLoadingSequence.ToReadOnlyReactivePropertySlim();
			this.ItemsSource = _webCrawlingService.GMarketFilterItems.ToReadOnlyReactiveCollection();

			this.HyperlinkCommand.Subscribe(this._webCrawlingService.OpenHyperlink);
			this.AddWishlistCommand.Subscribe(model => {
				this._webCrawlingService.AddItem(model);
			});
		}

		public async void OnNavigatedTo(NavigationContext navigationContext)
		{
			if (this.ItemsSource.Count == 0)
			{
				await Task.Run(this._webCrawlingService.SetGmarketHotDeal);
			}
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
		}
	}
}
