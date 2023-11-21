using HotDeal.Resources.Models;
using HotDeal.Services;
using Prism.Mvvm;
using Reactive.Bindings;

namespace HotDeal.ViewModels
{
	public class DanawaViewModel : BindableBase
	{
		private readonly WebCrawlingService _webCrawlingService;

		public ReadOnlyReactiveCollection<DanawaItem> DanawaList { get; set; }
		public ReadOnlyReactiveCollection<DanawaItem> DanawaFilterList { get; set; }

		public ReadOnlyReactivePropertySlim<bool> IsLoading { get; set; }

		public DanawaViewModel(WebCrawlingService webCrawlingService)
		{
			this._webCrawlingService = webCrawlingService;

			this.IsLoading = _webCrawlingService.IsLoading.ToReadOnlyReactivePropertySlim();
			this.DanawaList = _webCrawlingService.DanawaItems.ToReadOnlyReactiveCollection();
			this.DanawaFilterList = _webCrawlingService.DanawaFilterItems.ToReadOnlyReactiveCollection();
		}
	}
}
