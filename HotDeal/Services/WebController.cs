using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace HotDeal.Services
{
	public class WebController : IDisposable
	{
		public IWebDriver driver { get; init; }
		public ChromeDriverService driverService { get; init; }
		

		public WebController()
		{
			driverService = ChromeDriverService.CreateDefaultService();
			driverService.HideCommandPromptWindow = true;

			var chromeOptions = new ChromeOptions();
			chromeOptions.AddArgument("disable-gpu");
			chromeOptions.AddArgument("headless");

			driver = new ChromeDriver(driverService, chromeOptions);
		}

		public ReadOnlyCollection<IWebElement> GetList(string url, string path)
		{
			driver.Navigate().GoToUrl(url);
			var list = driver.FindElements(By.XPath(path));

			return list;
		}

		public void Dispose()
		{
			SynchronizationContext synchronizationContext = new SynchronizationContext();
			synchronizationContext.Post(_ =>
			{
				driver.Quit();
				driver.Dispose();
				driverService.Dispose();
			}, null);
		}
	}
}
