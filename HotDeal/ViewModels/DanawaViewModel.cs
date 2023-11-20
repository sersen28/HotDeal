﻿using HotDeal.Resources.Models;
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
		public ReadOnlyReactiveCollection<DanawaItem> DanawaFilterList { get; set; }

		public ReadOnlyReactivePropertySlim<bool> IsLoading { get; set; }

		public DanawaViewModel(WebCrawlingService webCrawlingService, UserService userService)
		{
			this._webCrawlingService = webCrawlingService;
			this._UserService = userService;

			this.IsLoading = _webCrawlingService.IsLoading.ToReadOnlyReactivePropertySlim();
			this.DanawaList = _webCrawlingService.DanawaItems.ToReadOnlyReactiveCollection();
			this.DanawaFilterList = _webCrawlingService.DanawaFilterItems.ToReadOnlyReactiveCollection();
		}
	}
}
