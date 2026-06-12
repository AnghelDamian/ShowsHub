using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShowsHub.Services;

namespace ShowsHub.ViewModels;
public partial class RegisterViewModel : ObservableObject
{
    private readonly MainWindowViewModel _nav;
    private readonly AuthService _authService = new();
    [ObservableProperty] private string username = string.Empty;
    [ObservableProperty] private string password = string.Empty;
    [ObservableProperty] private string message = string.Empty;

    public RegisterViewModel(MainWindowViewModel nav) { _nav = nav; }

    [RelayCommand]
    private void Register()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        { Message = "Completează toate câmpurile."; return; }
        if (_authService.Register(Username, Password))
        { Message = "Cont creat. Autentifică-te."; _nav.ShowLogin(); }
        else
            Message = "Utilizatorul există deja.";
    }

    [RelayCommand]
    private void GoLogin() => _nav.ShowLogin();
}
