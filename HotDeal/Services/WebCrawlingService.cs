using HotDeal.Resources.Constants;
using HotDeal.Resources.Models;
using HotDeal.Views;
using OpenQA.Selenium;
using Reactive.Bindings;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace HotDeal.Services
{
	public class WebCrawlingService 
	{
		private readonly FilterService _userService;
		private readonly LayoutService _layoutService;
		private CancellationTokenSource _token = new();

		public ReactiveCollection<HotDealModel> Wishlist { get; set; } = new();

		public ReactiveCollection<HotDealModel> DanawaDatas { get; set; } = new();
		public ReactivePropertySlim<LoadingSequence> DanawaLoadingSequence { get; set; } = new();

		public ReactiveCollection<HotDealModel> TmonFilterItems { get; set; } = new();
		public ReactivePropertySlim<LoadingSequence> TMonLoadingSequence { get; set; } = new();

		public ReactiveCollection<HotDealModel> GMarketFilterItems { get; set; } = new();
		public ReactivePropertySlim<LoadingSequence> GmarketLoadingSequence { get; set; } = new();

		public ReadOnlyReactiveProperty<HotDealFilter> UserFilter { get; set; }
		public ReactivePropertySlim<bool> AllowFilter { get; set; } = new(true);

		bool? _isAscending = null;
		string _ordered = string.Empty;

		public WebCrawlingService(FilterService userService, LayoutService layoutService) 
		{
			this._userService = userService;
			this._layoutService = layoutService;

			this.UserFilter = this._userService.UserFilter.ToReadOnlyReactiveProperty();
			this.DanawaLoadingSequence.Value = new("Danawa\nLoading");
			this.TMonLoadingSequence.Value = new("Tmon\nLoading");
			this.GmarketLoadingSequence.Value = new("GMarket\nLoading");

			InitializeList();
		}

		public void SwitchFilter()
		{
			this.AllowFilter.Value = !this.AllowFilter.Value;
		}

		private async void InitializeList()
		{
			this._layoutService.ChangeContentRegion("DanawaView");
			await Task.Run(SetDanawaHotDeal);
		}

		public async void Refresh()
		{
			switch (this._layoutService.DisplayContentName)
			{
				case nameof(DanawaView):
					await Task.Run(SetDanawaHotDeal);
					break;
				case nameof(TMonView):
					await Task.Run(SetTmonHotDealList);
					break;
				case nameof(GMarketView):
					await Task.Run(SetGmarketHotDeal);
					break;
			}
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

			DanawaDatas.ClearOnScheduler();
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
							var item = new HotDealModel(ReplaceDescription(description), price, discount, img, hyperlink);
							this.DanawaDatas.AddOnScheduler(item);
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
			if (this._isAscending is not null)
			{
				ListSort(_ordered, this._isAscending is true);
			}
		}

		#region Sort
		public void ListSort(string key, bool isAscending)
		{
			this._ordered = key;
			this._isAscending = isAscending;
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

		private bool GetList(out ReactiveCollection<HotDealModel> filtered)
		{
			switch (this._layoutService.DisplayContentName)
			{
				case nameof(DanawaView):
					filtered = this.DanawaDatas;
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

			HotDealModel[] src = null, ftd = null;
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

			HotDealModel[] src = null, ftd = null;
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

			HotDealModel[] src = null, ftd = null;
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
							var item = new HotDealModel(ReplaceDescription(description), price, discount, img, hyperlink);

							this.TmonFilterItems.AddOnScheduler(item);
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
			if (this._isAscending is not null)
			{
				ListSort(_ordered, this._isAscending is true);
			}
		}

		public void SetGmarketHotDeal()
		{
			var sequence = this.GmarketLoadingSequence.Value;
			sequence.IsLoading.Value = true;

			GMarketFilterItems.ClearOnScheduler();
			using (var controller = new WebController())
			{
				controller.driver.Navigate().GoToUrl("https://www.gmarket.co.kr/n/superdeal");
				var list = controller.driver.FindElements(By.XPath("//*[@id=\"container\"]/div[2]/ul/div/div/li"));
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
							var item = new HotDealModel(ReplaceDescription(description), price, discount, img, hyperlink);
							this.GMarketFilterItems.AddOnScheduler(item);
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
			if (this._isAscending is not null)
			{
				ListSort(_ordered, this._isAscending is true);
			}
		}

		private string ReplaceDescription(string description)
		{
			return description.Replace("<br>", "\r\n")
				.Replace("&lt;", "<")
				.Replace("&gt;", ">")
				.Replace("&amp;", "&")
				.Replace("&quot;", "\"");
		}
		
		public void AddItem(HotDealModel model)
		{
			Wishlist.AddOnScheduler(model);
			model.IsAddedWishList.Value = true;
		}

		public void DeleteItem(HotDealModel model)
		{

			if (this._layoutService.ShowModalMessageBox("Warning", "정말로 삭제하시겠습니까?", MessageBoxType.ConfirmOrCancel) is false)
			{
				return;
			}


			var item = Wishlist.Where(x => x.Description.Value.Equals(model.Description.Value)).FirstOrDefault();
			if (item is not null)
			{
				Wishlist.RemoveOnScheduler(item);
			}
			model.IsAddedWishList.Value = false;
		}
	}
}