using HotDeal.Resources.Models;
using HotDeal.Views;
using OpenQA.Selenium;
using Reactive.Bindings;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace HotDeal.Services
{
	public class WebCrawlingService 
	{
		private readonly UserService _userService;
		private readonly LayoutService _layoutService;
		private CancellationTokenSource _token = new();

		public ReactiveCollection<TMonModel> DanawaFilterItems { get; set; } = new();
		public ReactivePropertySlim<LoadingSequence> DanawaLoadingSequence { get; set; } = new();

		public ReactiveCollection<TMonModel> TmonFilterItems { get; set; } = new();
		public ReactivePropertySlim<LoadingSequence> TMonLoadingSequence { get; set; } = new();

		public ReactiveCollection<TMonModel> GMarketFilterItems { get; set; } = new();
		public ReactivePropertySlim<LoadingSequence> GmarketLoadingSequence { get; set; } = new();

		public ReactivePropertySlim<bool> IsDanawaLoading { get; set; } = new(true);
		public ReactivePropertySlim<bool> IsTMonLoading { get; set; } = new(true);
		public ReactivePropertySlim<bool> IsGMarketLoading { get; set; } = new(true);

		public ReadOnlyReactiveProperty<HotDealFilter> UserFilter { get; set; }
		
		public WebCrawlingService(UserService userService, LayoutService layoutService) 
		{
			this._userService = userService;
			this._layoutService = layoutService;

			this.UserFilter = this._userService.UserFilter.ToReadOnlyReactiveProperty();
			this.DanawaLoadingSequence.Value = new("Danawa\nLoading");
			this.TMonLoadingSequence.Value = new("Tmon\nLoading");
			this.GmarketLoadingSequence.Value = new("GMarket\nLoading");

			//InitializeList();
		}

		public void Cancel()
		{
			_token.Cancel();
		}

		public void OpenHyperlink(string link)
		{
			try
			{
				Process.Start(link);
			}
			catch
			{
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					link = link.Replace("&", "^&");
					Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				{
					Process.Start("xdg-open", link);
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					Process.Start("open", link);
				}
				else
				{
					throw;
				}
			}
		}

		public void SetDanawaHotDeal()
		{
			var sequence = this.DanawaLoadingSequence.Value;
			sequence.IsLoading.Value = true;

			DanawaFilterItems.ClearOnScheduler();
			using (var controller = new WebController())
			{
				controller.driver.Navigate().GoToUrl("https://www.danawa.com");
				var prod_list = controller.driver.FindElements(By.XPath("//*[@id=\"cmPickLayer\"]/div[2]/div[2]/div/ul/li"));
				sequence.Maximum.Value = prod_list.Count;
				foreach (var iter in prod_list)
				{
					try
					{
						var img = iter.FindElement(By.TagName("img")).GetAttribute("src");
						var hyperlink = iter.FindElement(By.XPath("a")).GetAttribute("href");
						var description = iter.FindElement(By.ClassName("prod-list__txt")).GetAttribute("innerHTML");
						var price_str = iter.FindElement(By.ClassName("num")).GetAttribute("innerHTML");
						var discount_str = iter.FindElement(By.ClassName("rate")).GetAttribute("innerHTML");


						if (ulong.TryParse(price_str.Replace(",", ""), out var price) && uint.TryParse(discount_str.Replace(",", ""), out var discount))
						{
							var item = new TMonModel(ReplaceDescription(description), price, discount, img, hyperlink);
							if (DanawaItemFilter(item))
							{
								this.DanawaFilterItems.AddOnScheduler(item);
							}
						}
					}
					catch (Exception e)
					{
						Debug.WriteLine($"({nameof(SetDanawaHotDeal)})" + e.Message);
					}

					if (_token.IsCancellationRequested)
					{
						controller.Dispose();
						return;
					}
					sequence.Current.Value++;
				}
			}
			sequence.Initialize();
		}

		#region Sort
		public void ListSort(string key, bool isAscending)
		{
			switch (key)
			{
				case "price":
					SortByPrice(isAscending);
					break;
				case "discount_percent":
					SortByPercent(isAscending);
					break;
				case "discount_price":
					SortByDiscount(isAscending);
					break;
			}
		}

		private bool GetList(out ReactiveCollection<TMonModel> filtered)
		{
			switch (this._layoutService.DisplayContentName)
			{
				case nameof(DanawaView):
					filtered = this.DanawaFilterItems;
					break;
				case nameof(TMonView):
					filtered = this.TmonFilterItems;
					break;
				case nameof(GMarketView):
					filtered = this.GMarketFilterItems;
					break;
				default:
					filtered = null;
					return false;
			}
			return true;
		}

		private void SortByPrice(bool isAscending)
		{
			if (!GetList(out var filtered))
			{
				return;
			}

			TMonModel[] src = null, ftd = null;
			if (isAscending)
			{
				ftd = filtered.OrderBy(x => x.Price.Value).ToArray();
			}
			else
			{
				ftd = filtered.OrderByDescending(x => x.Price.Value).ToArray();
			}

			filtered.ClearOnScheduler();
			filtered.AddRangeOnScheduler(ftd);
		}

		private void SortByPercent(bool isAscending)
		{
			if (!GetList(out var filtered))
			{
				return;
			}

			TMonModel[] src = null, ftd = null;
			if (isAscending)
			{
				ftd = filtered.OrderBy(x => x.Discount.Value).ToArray();
			}
			else
			{
				ftd = filtered.OrderByDescending(x => x.Discount.Value).ToArray();
			}

			filtered.ClearOnScheduler();
			filtered.AddRangeOnScheduler(ftd);
		}

		private void SortByDiscount(bool isAscending)
		{
			if (!GetList(out var filtered))
			{
				return;
			}

			TMonModel[] src = null, ftd = null;
			if (isAscending)
			{
				ftd = filtered.OrderBy(x => x.Reduce.Value).ToArray();
			}
			else
			{
				ftd = filtered.OrderByDescending(x => x.Reduce.Value).ToArray();
			}

			filtered.ClearOnScheduler();
			filtered.AddRangeOnScheduler(ftd);
		}
		#endregion


		private async void InitializeList()
		{
			await Task.Run(SetDanawaHotDeal);
		}

		public void SetTmonHotDealList()
		{
			var sequence = this.TMonLoadingSequence.Value;
			sequence.IsLoading.Value = true;

			TmonFilterItems.ClearOnScheduler();
			using (var controller = new WebController())
			{
				controller.driver.Navigate().GoToUrl("https://www.tmon.co.kr/planning/PLAN_elVpHLluqf");
				var list = controller.driver.FindElements(By.XPath("//*[@id=\"planContent\"]/div/div/div[2]/div[2]/div/ul/li"));
				sequence.Maximum.Value = list.Count;
				foreach (var iter in list)
				{
					try
					{
						var img = iter.FindElement(By.ClassName("thumb_image")).GetAttribute("data-src");
						var description = iter.FindElement(By.ClassName("title_name")).GetAttribute("innerHTML");
						var hyperlink = iter.FindElement(By.XPath("a")).GetAttribute("href");
						var price_str = iter.FindElement(By.XPath("a/div/div[2]/div[2]/span[1]/span[2]/i[1]")).GetAttribute("innerHTML");
						var original_price = iter.FindElement(By.XPath("a/div/div[2]/div[2]/span[1]/span[1]/i")).GetAttribute("innerHTML");
						var discount_str = iter.FindElement(By.XPath("a/div/div[2]/div[2]/strong/i")).GetAttribute("innerHTML");


						if (ulong.TryParse(price_str.Replace(",", ""), out var price) && uint.TryParse(discount_str.Replace(",", ""), out var discount))
						{
							var item = new TMonModel(ReplaceDescription(description), price, discount, img, hyperlink);
							if (DanawaItemFilter(item))
							{
								this.TmonFilterItems.AddOnScheduler(item);
							}
						}
					}
					catch (Exception e)
					{
						Debug.WriteLine($"({nameof(SetGmarketHotDeal)})" + e.Message);
						continue;
					}
					sequence.Current.Value++;
				}
			}
			sequence.Initialize();
		}

		public void SetGmarketHotDeal()
		{
			var sequence = this.GmarketLoadingSequence.Value;
			sequence.IsLoading.Value = true;

			GMarketFilterItems.ClearOnScheduler();
			using (var controller = new WebController())
			{
				controller.driver.Navigate().GoToUrl("https://www.gmarket.co.kr/n/superdeal");
				var list = controller.driver.FindElements(By.XPath("//*[@id=\"container\"]/div[2]/ul/li"));
				sequence.Maximum.Value = list.Count;
				foreach (var iter in list)
				{
					try
					{
						var img = iter.FindElement(By.XPath("div/a/img")).GetAttribute("src");
						var hyperlink = iter.FindElement(By.XPath("div/a")).GetAttribute("href");
						var description = iter.FindElement(By.XPath("div/a/span")).GetAttribute("innerHTML");
						var price_str = iter.FindElement(By.XPath("div/div[1]/span[1]/strong")).GetAttribute("innerHTML");
						var original_price = iter.FindElement(By.XPath("div/div[1]/span[1]/del")).GetAttribute("innerHTML");
						var discount_str = iter.FindElement(By.XPath("div/div[1]/em/strong")).GetAttribute("innerHTML");

						price_str = Regex.Replace(price_str, @"\D", "");
						original_price = Regex.Replace(original_price, @"\D", "");

						if (ulong.TryParse(price_str, out var price) && uint.TryParse(discount_str, out var discount))
						{
							var item = new TMonModel(ReplaceDescription(description), price, discount, img, hyperlink);
							if (DanawaItemFilter(item))
							{
								this.GMarketFilterItems.AddOnScheduler(item);
							}
						}
					}
					catch (Exception e)
					{
						Debug.WriteLine($"({nameof(SetGmarketHotDeal)})" + e.Message);
						continue;
					}
					sequence.Current.Value++;
				}
			}
			sequence.Initialize();
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

		private string ReplaceDescription(string description)
		{
			return description.Replace("<br>", "\r\n")
				.Replace("&lt;", "<")
				.Replace("&gt;", ">")
				.Replace("&amp;", "&")
				.Replace("&quot;", "\"");
		}
	}
}