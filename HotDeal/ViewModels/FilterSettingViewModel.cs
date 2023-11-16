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
	public class FilterSettingViewModel : BindableBase
	{
		private readonly UserService _UserService;

		public ReactiveProperty<uint> Discount { get; set; } = new(20);
		public ReactiveProperty<ulong> MinimumPrice { get; set; } = new(0);
		public ReactiveProperty<ulong> MaximumPrice { get; set; } = new(300000);

		public ReadOnlyReactiveProperty<HotDealFilter> UserFilter { get; set; }

		public ReactiveCommand SubmitCommand { get; set; } = new();

		public FilterSettingViewModel(UserService userService)
		{
			this._UserService = userService;

			this.UserFilter = this._UserService.UserFilter.ToReadOnlyReactiveProperty();

			this.SubmitCommand.Subscribe(() =>
			{
				this._UserService.SetUserFilter(
					discount: this.Discount.Value,
					min: this.MinimumPrice.Value,
					max: this.MaximumPrice.Value
				);
			});
		}
	}
}
