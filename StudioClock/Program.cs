using Avalonia;
using StudioClock.Services;

namespace StudioClock;

internal static class Program
{
    [STAThread]
    public static int Main(string[] args)
    {
        if (args is ["--settings-probe", var outputPath])
        {
            try
            {
                var settings = new SettingsService().Load();
                File.WriteAllText(outputPath, SettingsService.Serialize(settings));
                return 0;
            }
            catch { return 2; }
        }

        using var instance = new Services.SingleInstanceService("StudioClock-202608");
        if (!instance.IsPrimary)
        {
            instance.NotifyPrimary();
            return 0;
        }

        App.SingleInstance = instance;
        return BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UsePlatformDetect()
        .WithInterFont()
        .LogToTrace();
}
