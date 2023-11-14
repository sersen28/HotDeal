using HotDeal.Resources.Constants;
using HotDeal.Views;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;

namespace HotDeal.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
		private readonly IRegionManager _regionManager;

		public MainWindowViewModel(IRegionManager regionManager)
		{
			this._regionManager = regionManager;

		}
	}
}
