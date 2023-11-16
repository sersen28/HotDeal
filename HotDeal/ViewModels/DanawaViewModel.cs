using HotDeal.Resources.Models;
using HotDeal.Services;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HotDeal.ViewModels
{
	public class DanawaViewModel : BindableBase
	{
		private readonly WebCrawlingService _webCrawlingService;
		private readonly UserService _UserService;

		public ReadOnlyReactiveCollection<DanawaItem> DanawaList { get; set; }

		public ReactivePropertySlim<bool> IsLoading { get; set; } = new(true);

		public DanawaViewModel(WebCrawlingService webCrawlingService, UserService userService)
		{
			this._webCrawlingService = webCrawlingService;
			this._UserService = userService;

			this.DanawaList = _webCrawlingService.DanawaItems.ToReadOnlyReactiveCollection();
		}
	}
}
