using HotDeal.Resources.UI.Units;
using HotDeal.Views;
using Prism.Ioc;
using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotDeal.Services
{
    public class LayoutService
    {
		private readonly IRegionManager _regionManager;

		private PopupWindow _filterPopup;
		public ReactiveProperty<bool> isShowFilterPopup = new();

		public LayoutService(IRegionManager regionManager) 
        {
			this._regionManager = regionManager;
		}

        public void ShowDanawaView()
		{
			this._regionManager.RequestNavigate("ContentRegion", "DanawaView");
		}

		public void ShowGMarketView()
		{
			this._regionManager.RequestNavigate("ContentRegion", "GMarketView");
		}

		public void ShowTMonView()
		{
			this._regionManager.RequestNavigate("ContentRegion", "TMonView");
		}

		public void CloseAllPopupWindows()
		{
			if (this._filterPopup is not null)
			{
				this._filterPopup.Close();
				this._filterPopup = null;
			}
			this.isShowFilterPopup.Value = false;
		}

		public void ShowPopupWindow()
		{
			if (_filterPopup is null)
			{
				_filterPopup = new PopupWindow();
				_filterPopup.Closed += (s, e) =>
				{
					isShowFilterPopup.Value = false;
					_filterPopup = null;
				};
				_filterPopup.Show();
				isShowFilterPopup.Value = true;
			}
		}
	}
}
