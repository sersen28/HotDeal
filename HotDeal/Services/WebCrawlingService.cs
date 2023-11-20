using HotDeal.Resources.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reactive.Bindings;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HotDeal.Services
{
	public class WebCrawlingService
	{
		private readonly UserService _UserService;
		private ChromeDriver _driver;

		public ReactiveCollection<DanawaItem> DanawaItems { get; set; } = new();
		public ReactiveCollection<DanawaItem> DanawaFilterItems { get; set; } = new();

		public ReactivePropertySlim<bool> IsLoading { get; set; } = new(true);
		public ReadOnlyReactiveProperty<HotDealFilter> UserFilter { get; set; }

		public WebCrawlingService(UserService userService) 
		{
			this._UserService = userService;
			this.UserFilter = this._UserService.UserFilter.ToReadOnlyReactiveProperty();

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
			this.IsLoading.Value = true;
			_driver.Navigate().GoToUrl("https://www.danawa.com");
			var prod_list = _driver.FindElements(By.XPath("//*[@id=\"cmPickLayer\"]/div[2]/div[2]/div/ul/li"));
			foreach (var iter in prod_list)
			{
				try
				{
					var img = iter.FindElement(By.TagName("img")).GetAttribute("src");
					var description = iter.FindElement(By.ClassName("prod-list__txt")).GetAttribute("innerHTML");
					var price_str = iter.FindElement(By.ClassName("num")).GetAttribute("innerHTML");
					var discount_str = iter.FindElement(By.ClassName("rate")).GetAttribute("innerHTML");

					if (ulong.TryParse(price_str.Replace(",",""), out var price) && uint.TryParse(discount_str.Replace(",", ""), out var discount))
					{
						var item = new DanawaItem(ReplaceDescription(description), price, discount, img);
						this.DanawaItems.Add(item);
						if (DanawaItemFilter(item))
						{
							this.DanawaFilterItems.Add(item);
						}
					}
				}
				catch(Exception e) {
					Debug.WriteLine($"({nameof(InitDanawaHotDeal)})"+e.Message);
					continue;
				}
			}
			this.IsLoading.Value = false;
		}

		private bool DanawaItemFilter(DanawaItem item)
		{
			var filter = this.UserFilter.Value;
			if (item.Discount.Value < filter.Discount.Value
				|| item.Price.Value < filter.MinimumPrice.Value
				|| item.Price.Value > filter.MaximumPrice.Value)
			{
				return false;
			}
			return true;
		}

		private string ReplaceDescription(string description)
		{
			return description.Replace("<br>", "\n");
		}
	}
}