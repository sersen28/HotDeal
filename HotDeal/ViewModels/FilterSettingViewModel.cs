using HotDeal.Resources.Models;
using HotDeal.Services;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System.Threading;
using System.Threading.Tasks;

namespace HotDeal.ViewModels
{
	public class FilterSettingViewModel : BindableBase, INavigationAware
	{
		private readonly UserService _UserService;
		private readonly LayoutService _layoutService;
		private readonly WebCrawlingService _webCrawlingService;

		public ReactiveProperty<uint> Discount { get; set; } = new(20);
		public ReactiveProperty<ulong> MinimumPrice { get; set; } = new(0);
		public ReactiveProperty<ulong> MaximumPrice { get; set; } = new(300000);

		public ReadOnlyReactiveProperty<HotDealFilter> UserFilter { get; set; }

		public ReactiveCommand SubmitCommand { get; set; } = new();
		public ReactiveCommand PopupCommand { get; set; } = new();
		public AsyncReactiveCommand RefreshCommand { get; set; } = new();
		//public ReactiveCommand RefreshCommand { get; set; } = new();

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

			//this.RefreshCommand.Subscribe(async () =>
			//{
			//	_webCrawlingService.SetDanawaHotDeal();
			//});

			this.RefreshCommand.Subscribe(async () =>
			{
				await Task.Run(_webCrawlingService.SetDanawaHotDeal);
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
