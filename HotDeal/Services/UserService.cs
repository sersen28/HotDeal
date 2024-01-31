using HotDeal.Resources.Models;
using Reactive.Bindings;
using System.Configuration;

namespace HotDeal.Services
{
	public class UserService
    {
        private readonly LayoutService _layoutService;

		public ReactiveProperty<HotDealFilter> UserFilter { get; set; } = new();

		public UserService(LayoutService layoutService)
		{
			this._layoutService = layoutService;
			UserFilter.Value = new();
		}		

		public void SetUserFilter(uint discount, ulong min, ulong max)
		{
			this.UserFilter.Value.MaximumPrice.Value = max;
			this.UserFilter.Value.Discount.Value = discount;
			this.UserFilter.Value.MinimumPrice.Value = min;
			this.UserFilter.ForceNotify();

			if (min > max)
			{
				_layoutService.ShowModalMessageBox(
					title: "Warning",
					message: "최소 가격이 최대 가격보다 큽니다.\n다시 확인해 주세요.",
					type: Resources.Constants.MessageBoxType.Confirm
				);
			}
		}
	}
}
