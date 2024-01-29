using HotDeal.Services;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Diagnostics;

namespace HotDeal.ViewModels
{
	public class OrderButtonsViewModel : BindableBase
	{
		private readonly WebCrawlingService _webCrawlingService;

		public ReactivePropertySlim<bool> IsPriceAscending { get; set; } = new();
		public ReactivePropertySlim<bool> IsPriceChecked { get; set; } = new();

		public ReactivePropertySlim<bool> IsDiscountPercentAscending { get; set; } = new();
		public ReactivePropertySlim<bool> IsDiscountPercentChecked { get; set; } = new();

		public ReactivePropertySlim<bool> IsDiscountPriceAscending { get; set; } = new();
		public ReactivePropertySlim<bool> IsDiscountPriceChecked { get; set; } = new();

		public ReactiveCommand<object> OrderCommand { get; set; } = new();

		private string _ordered = string.Empty;

		public OrderButtonsViewModel(WebCrawlingService webCrawlingService)
		{
			this._webCrawlingService = webCrawlingService;

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
		}
	}
}
