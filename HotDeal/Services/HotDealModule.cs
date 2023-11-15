using HotDeal.ViewModels;
using HotDeal.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace HotDeal.Services
{
	public class HotDealModule : IModule
	{
		public void OnInitialized(IContainerProvider containerProvider)
		{
			var regionManager = containerProvider.Resolve<RegionManager>();
			regionManager.RegisterViewWithRegion("TitleRegion", typeof(TitleView));
			regionManager.RegisterViewWithRegion("MainMenuRegion", typeof(MainMenuView));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
			containerRegistry.RegisterForNavigation<TitleView, TitleViewModel>();
			containerRegistry.RegisterForNavigation<MainMenuView, MainMenuViewModel>();
		}
	}
}
