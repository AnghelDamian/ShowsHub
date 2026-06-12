using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShowsHub.Services;

namespace ShowsHub.ViewModels;
public class AccentOption
{
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";
    public string Preview { get; set; } = "";
}

public partial class SettingsViewModel : ObservableObject
{
    private readonly MainWindowViewModel _nav;
    private readonly SettingsService _settingsService = new();
    private readonly string _username;

    [ObservableProperty] private bool isLightTheme;
    [ObservableProperty] private string accent = "#FF7C3AED";
    [ObservableProperty] private string message = string.Empty;
    [ObservableProperty] private AccentOption? selectedAccent;

    public List<AccentOption> AccentOptions { get; } = new()
    {
        new AccentOption { Name = "Violet",     Value = "#FF7C3AED", Preview = "#7C3AED" },
        new AccentOption { Name = "Albastru",   Value = "#FF2563EB", Preview = "#2563EB" },
        new AccentOption { Name = "Cyan",       Value = "#FF0891B2", Preview = "#0891B2" },
        new AccentOption { Name = "Verde",      Value = "#FF059669", Preview = "#059669" },
        new AccentOption { Name = "Roz",        Value = "#FFDB2777", Preview = "#DB2777" },
        new AccentOption { Name = "Portocaliu", Value = "#FFEA580C", Preview = "#EA580C" },
        new AccentOption { Name = "Auriu",      Value = "#FFD97706", Preview = "#D97706" },
        new AccentOption { Name = "Roșu",       Value = "#FFDC2626", Preview = "#DC2626" },
    };

    public SettingsViewModel(MainWindowViewModel nav, string username)
    {
        _nav = nav;
        _username = username;
        var settings = _settingsService.Load();
        IsLightTheme = !settings.IsDark;
        Accent = settings.Accent;
        SelectedAccent = AccentOptions.FirstOrDefault(a => a.Value == Accent) ?? AccentOptions[0];
    }

    partial void OnSelectedAccentChanged(AccentOption? value)
    {
        if (value != null) Accent = value.Value;
    }

    [RelayCommand]
    private void Back() => _nav.ShowHome(_username);

    [RelayCommand]
    private void Save()
    {
        _settingsService.Save(new AppSettings { IsDark = !IsLightTheme, Accent = Accent });
        ThemeService.ApplyTheme(!IsLightTheme, Accent);
        Message = "✓ Setările au fost salvate.";
    }
}
