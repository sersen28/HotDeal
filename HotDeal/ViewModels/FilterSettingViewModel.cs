using HotDeal.Resources.Models;
using HotDeal.Services;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace HotDeal.ViewModels
{
	public class FilterSettingViewModel : BindableBase, INavigationAware
	{
		private readonly UserService _UserService;
		private readonly LayoutService _layoutService;
		private readonly WebCrawlingService _webCrawlingService;

		public ReactivePropertySlim<bool> IsPriceAscending { get; set; } = new();
		public ReactivePropertySlim<bool> IsPriceChecked { get; set; } = new();
		public ReactivePropertySlim<bool> IsDiscountPercentAscending { get; set; } = new();
		public ReactivePropertySlim<bool> IsDiscountPercentChecked { get; set; } = new();
		public ReactivePropertySlim<bool> IsDiscountPriceAscending { get; set; } = new();
		public ReactivePropertySlim<bool> IsDiscountPriceChecked { get; set; } = new();
		public ReactiveCommand<object> OrderCommand { get; set; } = new();
		private string _ordered = string.Empty;

		public ReactiveProperty<uint> Discount { get; set; } = new(20);
		public ReactiveProperty<ulong> MinimumPrice { get; set; } = new(0);
		public ReactiveProperty<ulong> MaximumPrice { get; set; } = new(300000);

		public ReactiveProperty<bool> UseFilter { get; set; } = new(true);

		public ReadOnlyReactiveProperty<HotDealFilter> UserFilter { get; set; }

		public ReactiveCommand<string> ShowFilterCommand { get; set; } = new();

		public ReactiveCommand SubmitCommand { get; set; } = new();
		public ReactiveCommand PopupCommand { get; set; } = new();
		public ReactiveCommand RefreshCommand { get; set; } = new();

		public FilterSettingViewModel(UserService userService, LayoutService layoutService, WebCrawlingService webCrawlingService)
		{
			this._UserService = userService;
			this._layoutService = layoutService;
			this._webCrawlingService = webCrawlingService;

			this.UserFilter = this._UserService.UserFilter.ToReadOnlyReactiveProperty();


			this.PopupCommand.Subscribe(() =>
			{
				_layoutService.ShowPopupWindow();
			});

			this.SubmitCommand.Subscribe(() =>
			{
				this._UserService.SetUserFilter(
					discount: this.Discount.Value,
					min: this.MinimumPrice.Value,
					max: this.MaximumPrice.Value
				);
			});

			this.OrderCommand.Subscribe((object param) => {
				if (param is string value)
				{
					Debug.WriteLine(value);
					switch (value)
					{
						case "price":
							if (this._ordered.Equals(value))
							{
								this.IsPriceAscending.Value = !this.IsPriceAscending.Value;
							}
							webCrawlingService.ListSort(value, IsPriceAscending.Value);
							break;
						case "discount_percent":
							if (this._ordered.Equals(value))
							{
								this.IsDiscountPercentAscending.Value = !this.IsDiscountPercentAscending.Value;
							}
							webCrawlingService.ListSort(value, IsDiscountPercentAscending.Value);
							break;
						case "discount_price":
							if (this._ordered.Equals(value))
							{
								this.IsDiscountPriceAscending.Value = !this.IsDiscountPriceAscending.Value;
							}
							webCrawlingService.ListSort(value, IsDiscountPriceAscending.Value);
							break;
					}
					this._ordered = value;
				}
			});

			this.RefreshCommand.Subscribe(this._webCrawlingService.Refresh);

			Initialize();
		}

		private void Initialize()
		{
			this.Discount.Value = this.UserFilter.Value.Discount.Value;
			this.MinimumPrice.Value = this.UserFilter.Value.MinimumPrice.Value;
			this.MaximumPrice.Value = this.UserFilter.Value.MaximumPrice.Value;
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			Initialize();
		}

		public bool IsNavigationTarget(NavigationContext navigationContext)
		{
			return true;
		}

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
			Initialize();
		}
	}
}
