using HotDeal.Resources.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reactive.Bindings;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HotDeal.Services
{
	public class WebCrawlingService : IDisposable
	{
		private readonly UserService _UserService;
		private ChromeDriver _driver;

		public ReactiveCollection<DanawaModel> DanawaItems { get; set; } = new();
		public ReactiveCollection<DanawaModel> DanawaFilterItems { get; set; } = new();

		public ReactiveCollection<TMonModel> TmonItems { get; set; } = new();
		public ReactiveCollection<TMonModel> TmonFilterItems { get; set; } = new();

		public ReactiveCollection<TMonModel> GMarketItems { get; set; } = new();
		public ReactiveCollection<TMonModel> GMarketFilterItems { get; set; } = new();

		public ReactivePropertySlim<bool> IsDanawaLoading { get; set; } = new(true);
		public ReactivePropertySlim<bool> IsTMonLoading { get; set; } = new(true);
		public ReactivePropertySlim<bool> IsGMarketLoading { get; set; } = new(true);
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
			Task.Run(() => {
				InitTmonHotDeal();
				InitGmarketHotDeal();
				InitDanawaHotDeal();
			});
		}

		private void InitTmonHotDeal()
		{
			this.IsTMonLoading.Value = true;
			_driver.Navigate().GoToUrl("https://www.tmon.co.kr/planning/PLAN_elVpHLluqf");
			var list = _driver.FindElements(By.XPath("//*[@id=\"planContent\"]/div/div/div[2]/div[2]/div/ul/li"));

			foreach (var iter in list)
			{
				try
				{
					var img = iter.FindElement(By.ClassName("thumb_image")).GetAttribute("data-src");
					var description = iter.FindElement(By.ClassName("title_name")).GetAttribute("innerHTML");
					var price_str = iter.FindElement(By.XPath("a/div/div[2]/div[2]/span[1]/span[2]/i[1]")).GetAttribute("innerHTML");
					var original_price = iter.FindElement(By.XPath("a/div/div[2]/div[2]/span[1]/span[1]/i")).GetAttribute("innerHTML");
					var discount_str = iter.FindElement(By.XPath("a/div/div[2]/div[2]/strong/i")).GetAttribute("innerHTML");


					if (ulong.TryParse(price_str.Replace(",", ""), out var price) && uint.TryParse(discount_str.Replace(",", ""), out var discount))
					{
						var item = new TMonModel(ReplaceDescription(description), price, discount, img);
						this.TmonItems.Add(item);
						if (DanawaItemFilter(item))
						{
							this.TmonFilterItems.Add(item);
						}
					}
				}
				catch (Exception e)
				{
					Debug.WriteLine($"({nameof(InitGmarketHotDeal)})" + e.Message);
					continue;
				}
			}
			this.IsTMonLoading.Value = false;
		}

		private void InitGmarketHotDeal()
		{
			this.IsGMarketLoading.Value = true;
			_driver.Navigate().GoToUrl("https://www.gmarket.co.kr/n/superdeal");
			var list = _driver.FindElements(By.XPath("//*[@id=\"container\"]/div[2]/ul/li"));
			foreach (var iter in list)
			{
				try
				{
					var img = iter.FindElement(By.XPath("div/a/img")).GetAttribute("src");
					var description = iter.FindElement(By.XPath("div/a/span")).GetAttribute("innerHTML");
					var price_str = iter.FindElement(By.XPath("div/div[1]/span[1]/strong")).GetAttribute("innerHTML");
					var original_price = iter.FindElement(By.XPath("div/div[1]/span[1]/del")).GetAttribute("innerHTML");
					var discount_str = iter.FindElement(By.XPath("div/div[1]/em/strong")).GetAttribute("innerHTML");

					price_str = Regex.Replace(price_str, @"\D", "");
					original_price = Regex.Replace(original_price, @"\D", "");

					//Debug.WriteLine($"{nameof(img)} : {img}");
					//Debug.WriteLine($"{nameof(description)} : {description}");
					//Debug.WriteLine($"{nameof(price_str)} : {Regex.Replace(price_str, @"\D", "")}");
					//Debug.WriteLine($"{nameof(original_price)} : {Regex.Replace(original_price, @"\D", "")}");
					//Debug.WriteLine($"{nameof(discount_str)} : {discount_str}");

					if (ulong.TryParse(price_str, out var price) && uint.TryParse(discount_str, out var discount))
					{
						var item = new TMonModel(ReplaceDescription(description), price, discount, img);
						this.GMarketItems.Add(item);
						if (DanawaItemFilter(item))
						{
							this.GMarketFilterItems.Add(item);
						}
					}
				}
				catch (Exception e)
				{
					Debug.WriteLine($"({nameof(InitGmarketHotDeal)})" + e.Message);
					continue;
				}
			}
			this.IsGMarketLoading.Value = false;
		}

		private void InitDanawaHotDeal()
		{
			this.IsDanawaLoading.Value = true;
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
						var item = new DanawaModel(ReplaceDescription(description), price, discount, img);
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
			this.IsDanawaLoading.Value = false;
		}

		private bool DanawaItemFilter(TMonModel item)
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

		private bool DanawaItemFilter(DanawaModel item)
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

		public void Dispose()
		{
			_driver.Quit();
		}
	}
}