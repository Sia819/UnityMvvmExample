using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

namespace UnityMvvmExample.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty] private string _displayName;
        [ObservableProperty] private string _name;

        [ObservableProperty] private ObservableCollection<string> _items = new ObservableCollection<string>();

        public MainViewModel()
        {
            Items.Add("item1");
        }

        [RelayCommand]
        private void Apply()
        {
            DisplayName = $"Hello, {Name}!";
            Debug.Log("Apply" + DisplayName);
        }

        [RelayCommand]
        private void AddItem()
        {
            Dispatcher.Enqueue(() => { 

            string str = Items.LastOrDefault();
            // itme1 in int parse to -> item2 -> itme3
            if (str != null)
            {
                int num = int.Parse(str.Substring(4)) + 1;
                Items.Add($"item{num}");
                Debug.Log("AddItem" + Items.LastOrDefault());
            }
            else
                Debug.LogWarning("??");
            });
        }
    }
}
