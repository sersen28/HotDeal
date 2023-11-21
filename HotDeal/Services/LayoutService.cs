using HotDeal.Views;
using Prism.Ioc;
using Prism.Regions;
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
	}
}
