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
		public ReactiveCommand MinimizeCommand { get; set; } = new();
		public ReactiveCommand MaximizeCommand { get; set; } = new();
		public ReactiveCommand RestoreCommand { get; set; } = new();

		public TitleViewModel()
		{
			this.CloseCommand.Subscribe(x => {
				if (x is Window window)
				{
					SystemCommands.CloseWindow(window);
				}
			});

			this.MinimizeCommand.Subscribe(x => {
				if (x is Window window)
				{
					SystemCommands.MinimizeWindow(window);
				}
			});

			this.MaximizeCommand.Subscribe(x => {
				if (x is Window window)
				{
					SystemCommands.MaximizeWindow(window);
				}
			});

			this.RestoreCommand.Subscribe(x => {
				if (x is Window window)
				{
					SystemCommands.RestoreWindow(window);
				}
			});
		}
	}
}
