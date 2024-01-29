using HotDeal.Resources.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows;

namespace HotDeal.Resources.Themes
{
	public class ThemeResolver : ResourceDictionary
    {
        private ObservableCollection<string> items = null;
        public static Uri GetAbsoluteUri(string AssemblyName, string path) => new Uri($"pack://application:,,,/{AssemblyName};Component/Themes/{path}");
        public static Uri GetAbsoluteUri(string path) => GetAbsoluteUri($"{HotDealText.ProjectTitle}.Resources", path);
        public Collection<string> Items
        {
            get
            {
                if (items == null)
                {
                    MergedDictionaries.Clear();
                    items = new ObservableCollection<string>();
                    items.CollectionChanged += OnItemsChanged;
                }
                return items;
            }
        }
        internal static Dictionary<string, ResourceDictionary> styles;

        private void OnItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            List<string>? oldItems = null;
            List<string>? newItems = null;
            if (e.Action != NotifyCollectionChangedAction.Reset)
            {
                if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
                {
                    if (e.OldItems != null)
                    {
                        oldItems = new List<string>(e.OldItems.Count);
                        foreach (var item in e.OldItems)
                        {
                            oldItems.Add((string)item);
                        }
                    }
                }
                if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
                {
                    if (e.NewItems != null)
                    {
                        newItems = new List<string>(e.NewItems.Count);
                        foreach (var item in e.NewItems)
                        {
                            newItems.Add((string)item);
                        }
                    }
                }
                if (oldItems != null)
                {
                    foreach (var item in oldItems)
                    {
                        if (styles.TryGetValue(item.ToLower(), out ResourceDictionary? resource))
                        {
                            MergedDictionaries.Remove(resource);
                        }
                    }
                }
                if (newItems != null)
                {
                    foreach (var item in newItems)
                    {
                        if (styles.TryGetValue(item.ToLower(), out ResourceDictionary? resource))
                        {
                            MergedDictionaries.Add(resource);
                        }
                    }
                }
            }
            else
            {
                MergedDictionaries.Clear();
                foreach (var item in styles)
                {
                    MergedDictionaries.Add(item.Value);
                }

            }
        }

        static ThemeResolver()
		{
			styles = new Dictionary<string, ResourceDictionary>();
			styles.Add("CommonDictionary".ToLower(), new ResourceDictionary() { Source = GetAbsoluteUri("CommonDictionary.xaml") });
			styles.Add("ColorDictionary".ToLower(), new ResourceDictionary() { Source = GetAbsoluteUri("ColorDictionary.xaml") });
			styles.Add("FontDictionary".ToLower(), new ResourceDictionary() { Source = GetAbsoluteUri("FontDictionary.xaml") });
			styles.Add("ImageDictionary".ToLower(), new ResourceDictionary() { Source = GetAbsoluteUri("ImageDictionary.xaml") });
			var asm = Assembly.GetExecutingAssembly();
			Stream stream = asm.GetManifestResourceStream(asm.GetName().Name + ".g.resources");
			using (var reader = new ResourceReader(stream))
			{
				foreach (DictionaryEntry entry in reader)
				{
					var itemKey = (string)entry.Key;
					if (itemKey.StartsWith("themes/units"))
					{
						var file = itemKey.Replace(".baml", ".xaml");
						string key = Path.GetFileNameWithoutExtension(file);
						string value = Path.GetFileName(file);
						styles.Add(key.ToLower(), new ResourceDictionary() { Source = GetAbsoluteUri($"units/{value}") });
					}
				}
			}
		}

        public ThemeResolver()
        {
            foreach (var item in styles)
            {
                MergedDictionaries.Add(item.Value);
            }
        }
    }
}
