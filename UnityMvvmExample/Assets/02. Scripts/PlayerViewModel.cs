using CommunityToolkit.Mvvm.ComponentModel;

public partial class PlayerViewModel : ObservableObject
{
    [ObservableProperty] private int _health;
    [ObservableProperty] private string _playerName;

    public PlayerViewModel()
    {

    }
}
