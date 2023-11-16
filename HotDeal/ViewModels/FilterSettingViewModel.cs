using HotDeal.Resources.Models;
using HotDeal.Services;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;

namespace HotDeal.ViewModels
{
	public class FilterSettingViewModel : BindableBase, INavigationAware
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
