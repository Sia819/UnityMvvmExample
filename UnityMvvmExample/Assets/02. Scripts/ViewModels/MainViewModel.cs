using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace UnityMvvmExample.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty] private string _displayName;
        [ObservableProperty] private string _name;

        [RelayCommand]
        private void Apply()
        {
            DisplayName = Name + "님 안녕하세요";
        }
    }
}
