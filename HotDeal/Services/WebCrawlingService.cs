using HotDeal.Resources.Models;
using HotDeal.Views;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reactive.Bindings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Xps.Serialization;

namespace HotDeal.Services
{
	public class WebCrawlingService 
	{
		private readonly UserService _userService;
		private readonly LayoutService _layoutService;
		private CancellationTokenSource _token = new();

		public ReactiveCollection<TMonModel> DanawaItems { get; set; } = new();
		public ReactiveCollection<TMonModel> DanawaFilterItems { get; set; } = new();
		public ReactivePropertySlim<LoadingSequence> DanawaLoadingSequence { get; set; } = new();

		public ReactiveCollection<TMonModel> TmonItems { get; set; } = new();
		public ReactiveCollection<TMonModel> TmonFilterItems { get; set; } = new();

		public ReactiveCollection<TMonModel> GMarketItems { get; set; } = new();
		public ReactiveCollection<TMonModel> GMarketFilterItems { get; set; } = new();

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

			//InitializeList();
		}

		public void Cancel()
		{
			_token.Cancel();
		}

		public void SetDanawaHotDeal()
		{
			var sequence = this.DanawaLoadingSequence.Value;

			sequence.IsLoading.Value = true;
			DanawaItems.ClearOnScheduler();
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
						var description = iter.FindElement(By.ClassName("prod-list__txt")).GetAttribute("innerHTML");
						var price_str = iter.FindElement(By.ClassName("num")).GetAttribute("innerHTML");
						var discount_str = iter.FindElement(By.ClassName("rate")).GetAttribute("innerHTML");


						if (ulong.TryParse(price_str.Replace(",", ""), out var price) && uint.TryParse(discount_str.Replace(",", ""), out var discount))
						{
							var item = new TMonModel(ReplaceDescription(description), price, discount, img);
							this.DanawaItems.AddOnScheduler(item);
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

		private bool GetList(out ReactiveCollection<TMonModel> source, out ReactiveCollection<TMonModel> filtered)
		{
			switch (this._layoutService.DisplayContentName)
			{
				case nameof(DanawaView):
					source = this.DanawaItems;
					filtered = this.DanawaFilterItems;
					break;
				case nameof(TMonView):
					source = this.TmonItems;
					filtered = this.TmonFilterItems;
					break;
				case nameof(GMarketView):
					source = this.GMarketItems;
					filtered = this.GMarketFilterItems;
					break;
				default:
					source = null;
					filtered = null;
					return false;
			}
			return true;
		}

		private void SortByPrice(bool isAscending)
		{
			if (!GetList(out var source, out var filtered))
			{
				return;
			}

			TMonModel[] src = null, ftd = null;
			if (isAscending)
			{
				src = source.OrderBy(x => x.Price.Value).ToArray();
				ftd = filtered.OrderBy(x => x.Price.Value).ToArray();
			}
			else
			{
				src = source.OrderByDescending(x => x.Price.Value).ToArray();
				ftd = filtered.OrderByDescending(x => x.Price.Value).ToArray();
			}

			source.ClearOnScheduler();
			filtered.ClearOnScheduler();
			source.AddRangeOnScheduler(src);
			filtered.AddRangeOnScheduler(ftd);
		}

		private void SortByPercent(bool isAscending)
		{
			if (!GetList(out var source, out var filtered))
			{
				return;
			}

			TMonModel[] src = null, ftd = null;
			if (isAscending)
			{
				src = source.OrderBy(x => x.Discount.Value).ToArray();
				ftd = filtered.OrderBy(x => x.Discount.Value).ToArray();
			}
			else
			{
				src = source.OrderByDescending(x => x.Discount.Value).ToArray();
				ftd = filtered.OrderByDescending(x => x.Discount.Value).ToArray();
			}

			source.ClearOnScheduler();
			filtered.ClearOnScheduler();
			source.AddRangeOnScheduler(src);
			filtered.AddRangeOnScheduler(ftd);
		}

		private void SortByDiscount(bool isAscending)
		{
			if (!GetList(out var source, out var filtered))
			{
				return;
			}

			TMonModel[] src = null, ftd = null;
			if (isAscending)
			{
				src = source.OrderBy(x => x.Reduce.Value).ToArray();
				ftd = filtered.OrderBy(x => x.Reduce.Value).ToArray();
			}
			else
			{
				src = source.OrderByDescending(x => x.Reduce.Value).ToArray();
				ftd = filtered.OrderByDescending(x => x.Reduce.Value).ToArray();
			}

			source.ClearOnScheduler();
			filtered.ClearOnScheduler();
			source.AddRangeOnScheduler(src);
			filtered.AddRangeOnScheduler(ftd);
		}
		#endregion


		private async void InitializeList()
		{
			await Task.Run(SetDanawaHotDeal);
		}

		private void SetTmonHotDealList()
		{
			this.IsTMonLoading.Value = true;
			ReadOnlyCollection<IWebElement> list;

			using (var controller = new WebController())
			{
				controller.driver.Navigate().GoToUrl("https://www.tmon.co.kr/planning/PLAN_elVpHLluqf");
				list = controller.driver.FindElements(By.XPath("//*[@id=\"planContent\"]/div/div/div[2]/div[2]/div/ul/li"));

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
						Debug.WriteLine($"({nameof(SetGmarketHotDeal)})" + e.Message);
						continue;
					}
				}
			}

			this.IsTMonLoading.Value = false;
		}

		private void SetGmarketHotDeal()
		{
			this.IsGMarketLoading.Value = true;
			using (var controller = new WebController())
			{
				controller.driver.Navigate().GoToUrl("https://www.gmarket.co.kr/n/superdeal");
				var list = controller.driver.FindElements(By.XPath("//*[@id=\"container\"]/div[2]/ul/li"));
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
						Debug.WriteLine($"({nameof(SetGmarketHotDeal)})" + e.Message);
						continue;
					}
				}
			}
			this.IsGMarketLoading.Value = false;
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
			return description.Replace("<br>", "");
		}
	}
}