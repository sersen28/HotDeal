using HotDeal.Services;
using HotDeal.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace HotDeal
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		public App()
		{
			this.Dispatcher.UnhandledException += this.OnDispatcherUnhandledException;
		}

		protected override Window CreateShell()
		{
			return Container.Resolve<MainWindow>();
		}

		protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
		{
			moduleCatalog.AddModule<HotDealModule>();
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterSingleton<WebCrawlingService>();
			containerRegistry.RegisterSingleton<UserService>();
			containerRegistry.RegisterSingleton<LayoutService>();
		}

		public void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{


		}

		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);
		}
	}
}
