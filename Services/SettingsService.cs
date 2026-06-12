using System.Text.Json;

namespace ShowsHub.Services;
public class SettingsService
{
    private readonly string _path;
    public SettingsService()
    {
        var folder = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "ShowsHub");
        System.IO.Directory.CreateDirectory(folder);
        _path = System.IO.Path.Combine(folder, "settings.json");
    }
    public AppSettings Load()
    {
        if (!System.IO.File.Exists(_path)) return new AppSettings();
        var t = System.IO.File.ReadAllText(_path);
        return JsonSerializer.Deserialize<AppSettings>(t) ?? new AppSettings();
    }
    public void Save(AppSettings s) => System.IO.File.WriteAllText(_path, JsonSerializer.Serialize(s));
}

public class AppSettings
{
    public bool IsDark { get; set; }
    public string Accent { get; set; } = "#FF007ACC";
}
