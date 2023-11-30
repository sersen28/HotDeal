using HotDeal.Services;
using HotDeal.Views;
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
		public ReactiveCommand WishlistCommand { get; set; } = new();

		public MainMenuViewModel(LayoutService layoutService)
		{
			this._layoutService = layoutService;

			this.DanawaCommand.Subscribe(() =>
			{
				this._layoutService.ChangeContentRegion(nameof(DanawaView));
			});

			this.TmonCommand.Subscribe(() =>
			{
				this._layoutService.ChangeContentRegion(nameof(TMonView));
			});

			this.GmarketCommand.Subscribe(() =>
			{
				this._layoutService.ChangeContentRegion(nameof(GMarketView));
			});

			this.WishlistCommand.Subscribe(() =>
			{
				this._layoutService.ChangeContentRegion(nameof(WishlistView));
			});
		}
	}
}
