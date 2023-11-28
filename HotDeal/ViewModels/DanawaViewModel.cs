using HotDeal.Resources.Models;
using HotDeal.Services;
using Prism.Mvvm;
using Reactive.Bindings;

namespace HotDeal.ViewModels
{
	public class DanawaViewModel : BindableBase
	{
		private readonly WebCrawlingService _webCrawlingService;
		private readonly LayoutService _layoutService;

		public ReadOnlyReactiveCollection<TMonModel> DanawaList { get; set; }
		public ReadOnlyReactiveCollection<TMonModel> DanawaFilterList { get; set; }

		public ReadOnlyReactivePropertySlim<bool> IsShowPopup { get; set; }
		public ReadOnlyReactivePropertySlim<bool> IsLoading { get; set; }
		public ReadOnlyReactivePropertySlim<LoadingSequence> LoadingSequence { get; set; }

		public DanawaViewModel(WebCrawlingService webCrawlingService, LayoutService layoutService)
		{
			this._webCrawlingService = webCrawlingService;
			this._layoutService = layoutService;

			this.LoadingSequence = _webCrawlingService.DanawaLoadingSequence.ToReadOnlyReactivePropertySlim();
			this.IsShowPopup = this._layoutService.IsShowFilterPopup.ToReadOnlyReactivePropertySlim();
			this.IsLoading = _webCrawlingService.IsDanawaLoading.ToReadOnlyReactivePropertySlim();
			this.DanawaList = _webCrawlingService.DanawaItems.ToReadOnlyReactiveCollection();
			this.DanawaFilterList = _webCrawlingService.DanawaFilterItems.ToReadOnlyReactiveCollection();
	
		}
	}
}
