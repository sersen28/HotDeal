using HotDeal.Resources.Constants;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Windows;

namespace HotDeal.ViewModels
{
	public class TitleViewModel : BindableBase
	{
		public ReactivePropertySlim<string> Title { get; set; } = new(HotDealText.ProjectTitle);
		public ReactiveCommand CloseCommand { get; set; } = new();

		public TitleViewModel()
		{
			CloseCommand.Subscribe(x=>{
				Application.Current.Shutdown();
			});
		}
	}
}
