using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;

namespace ShowsHub.Services;
public static class ThemeService
{
    public static void ApplyTheme(bool isDark, string accent)
    {
        var resources = Application.Current?.Resources;
        if (resources == null) return;

        Color accentColor;
        try { accentColor = Color.Parse(accent); }
        catch { accentColor = Color.Parse("#FF5B9DFF"); }

        if (isDark)
        {
            resources["AppBackground"] = new SolidColorBrush(Color.Parse("#FF0F172A"));
            resources["CardBackground"] = new SolidColorBrush(Color.Parse("#FF111827"));
            resources["PanelBackground"] = new SolidColorBrush(Color.Parse("#FF1F2937"));
            resources["TextBrush"] = new SolidColorBrush(Color.Parse("#FFFFFFFF"));
            resources["TextSecondaryBrush"] = new SolidColorBrush(Color.Parse("#FF94A3B8"));
        }
        else
        {
            resources["AppBackground"] = new SolidColorBrush(Color.Parse("#FFF8FAFC"));
            resources["CardBackground"] = new SolidColorBrush(Color.Parse("#FFFFFFFF"));
            resources["PanelBackground"] = new SolidColorBrush(Color.Parse("#FFF1F5F9"));
            resources["TextBrush"] = new SolidColorBrush(Color.Parse("#FF0F172A"));
            resources["TextSecondaryBrush"] = new SolidColorBrush(Color.Parse("#FF475569"));
        }

        resources["AccentBrush"] = new SolidColorBrush(accentColor);
        resources["AccentLightBrush"] = new SolidColorBrush(Color.FromArgb((byte)200, accentColor.R, accentColor.G, accentColor.B));
    }
}
