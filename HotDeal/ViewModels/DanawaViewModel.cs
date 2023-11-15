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
	public class DanawaViewModel : BindableBase
	{
		private readonly WebCrawlingService _webCrawlingService;

		public ReadOnlyReactiveCollection<DanawaItem> DanawaList { get; set; }

		public DanawaViewModel(WebCrawlingService webCrawlingService)
		{
			this._webCrawlingService = webCrawlingService;
			this.DanawaList = _webCrawlingService.DanawaItems.ToReadOnlyReactiveCollection();
		}
	}
}
