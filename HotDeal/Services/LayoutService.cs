using HotDeal.Items;
using HotDeal.Resources.Constants;
using HotDeal.Resources.UI.Units;
using HotDeal.Views;
using Prism.Ioc;
using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HotDeal.Services
{
    public class LayoutService
    {
		private readonly IRegionManager _regionManager;

		private PopupWindow _filterPopup;
		public ReactiveProperty<bool> IsShowFilterPopup = new();
		public string DisplayContentName = "DanawaView";

		public LayoutService(IRegionManager regionManager) 
        {
			this._regionManager = regionManager;
			ChangeRegion("ContentRegion", nameof(DanawaView));
		}

		public void ChangeContentRegion(string viewName)
		{
			ChangeRegion("ContentRegion", viewName);
		}

		public void ChangeRegion(string regionName, string viewName)
		{
			this._regionManager.RequestNavigate(regionName, viewName);
			this.DisplayContentName = viewName;
		}

		public void CloseAllPopupWindows()
		{
			if (this._filterPopup is not null)
			{
				this._filterPopup.Close();
				this._filterPopup = null;
			}
			this.IsShowFilterPopup.Value = false;
		}


		public bool? ShowModalMessageBox(string message)
		{
			return ShowModalMessageBox(string.Empty, message, MessageBoxType.Confirm);
		}

		public bool? ShowModalMessageBox(string title, string message, MessageBoxType type)
		{
			var msg = new HotDealMessageBox
			{
				WindowStartupLocation=System.Windows.WindowStartupLocation.CenterScreen,
				Message = message,
				MessageTitle = title,
				Type=type
			};
			return msg.ShowDialog();
		}

		public void ShowPopupWindow()
		{
			if (_filterPopup is null)
			{
				_filterPopup = new PopupWindow();
				_filterPopup.Closed += (s, e) =>
				{
					IsShowFilterPopup.Value = false;
					_filterPopup = null;
				};
				_filterPopup.Show();
				IsShowFilterPopup.Value = true;
			};
		}
	}
}
