using Reactive.Bindings;

namespace HotDeal.Resources.Models
{
	public class LoadingSequence
	{
		public ReactiveProperty<string> Name { get; init; } = new();
		public ReactiveProperty<string> Status { get; set; } = new();

		public ReactiveProperty<int> Current { get; set; } = new();
		public ReactiveProperty<int> Maximum { get; set; } = new(100);
		public ReactiveProperty<bool> IsLoading { get; set; } = new();

		public LoadingSequence(string name)
		{
			this.Name.Value = name;
		}

		public void Initialize()
		{
			this.Status.Value = string.Empty;
			this.Current.Value = 0;
			this.Maximum.Value = 100;
			this.IsLoading.Value = false;
		}
	}
}
