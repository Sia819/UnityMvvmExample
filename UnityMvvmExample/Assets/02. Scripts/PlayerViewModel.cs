// Usage in ViewModel
using System.ComponentModel;
using System.Runtime.CompilerServices;

public partial class PlayerViewModel
{
    [ObservableProperty]
    public int Health { get; set; } = 100;

    [ObservableProperty]
    public string PlayerName { get; set; } = "Player";
}
