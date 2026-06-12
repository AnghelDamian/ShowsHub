using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShowsHub.Services;

namespace ShowsHub.ViewModels;
public partial class LoginViewModel : ObservableObject
{
    private readonly MainWindowViewModel _nav;
    private readonly AuthService _authService = new();
    [ObservableProperty] private string username = string.Empty;
    [ObservableProperty] private string password = string.Empty;
    [ObservableProperty] private string message = string.Empty;

    public LoginViewModel(MainWindowViewModel nav) { _nav = nav; }

    [RelayCommand]
    private void Login()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        { Message = "Completează toate câmpurile."; return; }
        if (_authService.Login(Username, Password))
            _nav.ShowHome(Username);
        else
            Message = "Autentificare eșuată. Încearcă din nou.";
    }

    [RelayCommand]
    private void GoRegister() => _nav.ShowRegister();
}
