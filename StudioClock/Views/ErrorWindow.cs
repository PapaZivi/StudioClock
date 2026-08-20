using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
namespace StudioClock.Views;
public sealed class ErrorWindow : Window
{
    public ErrorWindow(string message)
    {
        Title = "StudioClock – Fehler"; Width = 520; SizeToContent = SizeToContent.Height; CanResize = false; WindowStartupLocation = WindowStartupLocation.CenterOwner;
        var ok = new Button { Content = "OK", HorizontalAlignment = HorizontalAlignment.Right }; ok.Click += (_, _) => Close();
        Content = new StackPanel { Margin = new Thickness(24), Spacing = 16, Children = { new TextBlock { Text = message, TextWrapping = Avalonia.Media.TextWrapping.Wrap }, ok } };
    }
}
