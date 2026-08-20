using System.Text.Json;
using StudioClock.Helpers;
using StudioClock.Models;
using StudioClock.Serialization;

namespace StudioClock.Services;

public sealed class SettingsService
{
    private readonly string _path;
    public SettingsService(string? path = null) => _path = path ?? GetDefaultPath();
    public string Path => _path;
    public string? LastSaveError { get; private set; }

    public AppSettings Load()
    {
        try
        {
            if (!File.Exists(_path)) return new AppSettings();
            return SettingsValidator.Normalize(JsonSerializer.Deserialize(File.ReadAllText(_path), AppJsonSerializerContext.Default.AppSettings));
        }
        catch { return new AppSettings(); }
    }

    public bool Save(AppSettings settings)
    {
        try
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_path)!);
            var temp = _path + ".tmp";
            var json = Serialize(SettingsValidator.Normalize(settings));
            using (var stream = new FileStream(temp, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(stream)) { writer.Write(json); writer.Flush(); stream.Flush(true); }
            File.Move(temp, _path, true);
            LastSaveError = null;
            return true;
        }
        catch (Exception exception)
        {
            LastSaveError = $"Einstellungen konnten nicht in '{_path}' gespeichert werden: {exception.Message}";
            return false;
        }
    }

    internal static string Serialize(AppSettings settings) =>
        JsonSerializer.Serialize(settings, AppJsonSerializerContext.Default.AppSettings);

    private static string GetDefaultPath() => System.IO.Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StudioClock", "settings.json");
}
