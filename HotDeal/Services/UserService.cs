using HotDeal.Resources.Models;
using Reactive.Bindings;

namespace HotDeal.Services
{
	public class UserService
    {
        public ReactiveProperty<HotDealFilter> UserFilter { get; set; } = new();

        public UserService() 
        {
            UserFilter.Value = new();
        }

        public void SetUserFilter(uint discount, ulong min, ulong max)
		{
			this.UserFilter.Value.MaximumPrice.Value = max;
			this.UserFilter.Value.Discount.Value = discount;
			this.UserFilter.Value.MinimumPrice.Value = min;
		}
    }
}
