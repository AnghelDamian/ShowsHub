using CommunityToolkit.Mvvm.ComponentModel;
using ShowsHub.Views;

namespace ShowsHub.ViewModels;
public partial class MainWindowViewModel : ObservableObject
{
    private object _currentView = new object();
    public object CurrentView { get => _currentView; set => SetProperty(ref _currentView, value); }

    public MainWindowViewModel() => ShowLogin();

    public void ShowHome(string username)
    {
        var vm = new HomeViewModel(this, username);
        var view = new HomeView { DataContext = vm };
        CurrentView = view;
    }

    public void ShowLogin()
    {
        var vm = new LoginViewModel(this);
        var view = new LoginView { DataContext = vm };
        CurrentView = view;
    }

    public void ShowRegister()
    {
        var vm = new RegisterViewModel(this);
        var view = new RegisterView { DataContext = vm };
        CurrentView = view;
    }

    public void ShowDetails(Models.MediaItem item, string username)
    {
        var vm = new DetailsViewModel(this, item, username);
        var view = new DetailsView { DataContext = vm };
        CurrentView = view;
    }

    public void ShowSettings(string username)
    {
        var vm = new SettingsViewModel(this, username);
        var view = new SettingsView { DataContext = vm };
        CurrentView = view;
    }
}
