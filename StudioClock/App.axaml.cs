using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using StudioClock.Platform;
using StudioClock.Services;
using StudioClock.Views;

namespace StudioClock;

public partial class App : Application
{
    internal static SingleInstanceService? SingleInstance { get; set; }
    private AppController? _controller;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            var settingsService = new SettingsService();
            var settings = settingsService.Load();
            IStartupService startup = OperatingSystem.IsWindows()
                ? new StudioClock.Platform.Windows.WindowsStartupService()
                : new StudioClock.Platform.MacOS.MacStartupService();
            var window = new MainWindow(settings);
            _controller = new AppController(desktop, window, settings, settingsService, startup);
            window.Controller = _controller;
            desktop.MainWindow = window;
            desktop.Exit += (_, _) => _controller.Dispose();
            SingleInstance!.ActivationRequested += () => Dispatcher.UIThread.Post(_controller.Show);
            SingleInstance.StartListening();
            _controller.Initialize();
        }
        base.OnFrameworkInitializationCompleted();
    }
}
