using System.Text.Json;
using StudioClock.Helpers;
using StudioClock.Models;

namespace StudioClock.Services;

public sealed class SettingsService
{
    private readonly string _path;
    public SettingsService(string? path = null) => _path = path ?? GetDefaultPath();
    public string Path => _path;

    public AppSettings Load()
    {
        try
        {
            if (!File.Exists(_path)) return new AppSettings();
            return SettingsValidator.Normalize(JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(_path)));
        }
        catch { return new AppSettings(); }
    }

    public void Save(AppSettings settings)
    {
        try
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_path)!);
            var temp = _path + ".tmp";
            File.WriteAllText(temp, JsonSerializer.Serialize(SettingsValidator.Normalize(settings), new JsonSerializerOptions { WriteIndented = true }));
            File.Move(temp, _path, true);
        }
        catch { /* Settings must never terminate the clock. */ }
    }

    private static string GetDefaultPath() => System.IO.Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StudioClock", "settings.json");
}

