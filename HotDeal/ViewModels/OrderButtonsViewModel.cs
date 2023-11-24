using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HotDeal.ViewModels
{
	public class OrderButtonsViewModel : BindableBase
	{

		public ReactivePropertySlim<bool> IsPriceAscending { get; set; } = new();
		public ReactivePropertySlim<bool> IsPriceChecked { get; set; } = new();

		public ReactivePropertySlim<bool> IsDiscountPercentAscending { get; set; } = new();
		public ReactivePropertySlim<bool> IsDiscountPercentChecked { get; set; } = new();

		public ReactivePropertySlim<bool> IsDiscountPriceAscending { get; set; } = new();
		public ReactivePropertySlim<bool> IsDiscountPriceChecked { get; set; } = new();

		public ReactiveCommand<object> OrderCommand { get; set; } = new();

		private string _ordered = string.Empty;

		public OrderButtonsViewModel()
		{
			this.OrderCommand.Subscribe((object param) => {
				if (param is string value)
				{
					Debug.WriteLine(value);
					switch (value)
					{
						case "price":
							Debug.WriteLine(IsPriceChecked.Value);
							if (this._ordered.Equals(value))
							{
								this.IsPriceAscending.Value = !this.IsPriceAscending.Value;
							}
							break;
						case "discount_percent":
							Debug.WriteLine(IsDiscountPercentAscending.Value);
							if (this._ordered.Equals(value))
							{
								this.IsDiscountPercentAscending.Value = !this.IsDiscountPercentAscending.Value;
							}
							break;
						case "discount_price":
							Debug.WriteLine(IsDiscountPriceChecked.Value);
							if (this._ordered.Equals(value))
							{
								this.IsDiscountPriceAscending.Value = !this.IsDiscountPriceAscending.Value;
							}
							break;
					}
					this._ordered = value;
				}
			});
		}
	}
}
