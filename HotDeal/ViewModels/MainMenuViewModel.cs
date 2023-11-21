using HotDeal.Services;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HotDeal.ViewModels
{
	public class MainMenuViewModel : BindableBase
	{
		private readonly LayoutService _layoutService;

		public ReactiveCommand DanawaCommand { get; set; } = new();
		public ReactiveCommand TmonCommand { get; set; } = new();
		public ReactiveCommand GmarketCommand { get; set; } = new();

		public MainMenuViewModel(LayoutService layoutService)
		{
			this._layoutService = layoutService;

			this.DanawaCommand.Subscribe(() =>
			{
				this._layoutService.ShowDanawaView();
			});

			this.TmonCommand.Subscribe(() =>
			{
				this._layoutService.ShowTMonView();
			});

			this.GmarketCommand.Subscribe(() =>
			{
				this._layoutService.ShowGMarketView();
			});
		}
	}
}
