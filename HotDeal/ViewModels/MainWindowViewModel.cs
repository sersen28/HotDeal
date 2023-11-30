using HotDeal.Services;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;

namespace HotDeal.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
		private readonly IRegionManager _regionManager;
		private readonly WebCrawlingService _webCrawlingService;
		private readonly LayoutService _layoutService;

		public ReactiveCommand ClosingCommand { get; set; } = new();

		public MainWindowViewModel(IRegionManager regionManager, WebCrawlingService webCrawlingService, LayoutService layoutService)
		{
			this._regionManager = regionManager;
			this._webCrawlingService = webCrawlingService;
			this._layoutService = layoutService;

			this.ClosingCommand.Subscribe(OnClosing);
		}

		private void OnClosing()
		{
			this._webCrawlingService.Cancel();
			this._layoutService.CloseAllPopupWindows();
		}
	}
}
