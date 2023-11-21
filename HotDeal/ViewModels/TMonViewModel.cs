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

		public ReadOnlyReactiveCollection<TMonModel> DanawaList { get; set; }
		public ReadOnlyReactiveCollection<TMonModel> DanawaFilterList { get; set; }

		public ReadOnlyReactivePropertySlim<bool> IsLoading { get; set; }

		public TMonViewModel(WebCrawlingService webCrawlingService)
		{
			this._webCrawlingService = webCrawlingService;

			this.IsLoading = _webCrawlingService.IsTMonLoading.ToReadOnlyReactivePropertySlim();
			this.DanawaList = _webCrawlingService.TmonItems.ToReadOnlyReactiveCollection();
			this.DanawaFilterList = _webCrawlingService.TmonFilterItems.ToReadOnlyReactiveCollection();
		}
	}
}
