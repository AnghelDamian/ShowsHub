using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ShowsHub.Services;

namespace ShowsHub;
public class App : Application
{
    private readonly SettingsService _settingsService = new();

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        var settings = _settingsService.Load();
        ThemeService.ApplyTheme(settings.IsDark, settings.Accent);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var main = new MainWindow();
            main.DataContext = new ViewModels.MainWindowViewModel();
            desktop.MainWindow = main;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
