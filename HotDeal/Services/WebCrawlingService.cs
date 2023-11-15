using HotDeal.Resources.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotDeal.Services
{
	public class WebCrawlingService
	{
		private ChromeDriver _driver;

		public ReactiveCollection<DanawaItem> DanawaItems = new();

		public WebCrawlingService() 
		{
			var driverService = ChromeDriverService.CreateDefaultService();
			driverService.HideCommandPromptWindow = true;

			var opt = new ChromeOptions();
			opt.AddArgument("disable-gpu");
			opt.AddArgument("headless");

			_driver = new ChromeDriver(driverService, opt);
			Task.Run(InitDanawaHotDeal);
		}

		private void InitDanawaHotDeal()
		{
			_driver.Navigate().GoToUrl("https://www.danawa.com");
			var prod_list = _driver.FindElements(By.XPath("//*[@id=\"cmPickLayer\"]/div[2]/div[2]/div/ul/li"));
			foreach (var iter in prod_list)
			{
				try
				{
					var img = iter.FindElement(By.TagName("img")).GetAttribute("src");
					var description = iter.FindElement(By.ClassName("prod-list__txt")).GetAttribute("innerHTML");
					var price = iter.FindElement(By.ClassName("num")).GetAttribute("innerHTML");
					var discount = iter.FindElement(By.ClassName("rate")).GetAttribute("innerHTML");

					this.DanawaItems.Add(new DanawaItem(ReplaceDescription(description), price, discount, img));
				}
				catch(Exception e) {
					Debug.WriteLine($"({nameof(InitDanawaHotDeal)})"+e.Message);
					continue;
				}
			}
		}

		private string ReplaceDescription(string description)
		{
			return description.Replace("<br>", "\n");
		}
	}
}