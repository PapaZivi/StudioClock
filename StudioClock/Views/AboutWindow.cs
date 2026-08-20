using System.Reflection;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using StudioClock.Helpers;

namespace StudioClock.Views;
public sealed class AboutWindow : Window
{
    public AboutWindow()
    {
        Title = "About StudioClock"; Width = 410; Height = 300; CanResize = false; WindowStartupLocation = WindowStartupLocation.CenterOwner;
        var assembly = Assembly.GetExecutingAssembly();
        var rawVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? assembly.GetName().Version?.ToString();
        var version = VersionHelper.DisplayVersion(rawVersion);
        var close = new Button { Content = "Schließen", HorizontalAlignment = HorizontalAlignment.Center }; close.Click += (_, _) => Close();
        Content = new StackPanel { Margin = new Thickness(28), Spacing = 12, HorizontalAlignment = HorizontalAlignment.Center, Children =
        {
            new TextBlock { Text = "StudioClock", FontSize = 28, FontWeight = Avalonia.Media.FontWeight.Bold },
            new TextBlock { Text = $"Version {version}" }, new TextBlock { Text = "© 2026 StudioClock contributors" },
            new TextBlock { Text = $".NET {Environment.Version}" }, new TextBlock { Text = RuntimeInformation.OSDescription, TextWrapping = Avalonia.Media.TextWrapping.Wrap }, close
        }};
    }
}
