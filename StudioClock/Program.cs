using Avalonia;

namespace StudioClock;

internal static class Program
{
    [STAThread]
    public static int Main(string[] args)
    {
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

