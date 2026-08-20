using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using StudioClock.Services;

namespace StudioClock.Platform.MacOS;
public sealed class AvaloniaColorPickerService : IColorPickerService
{
    public async Task<Color?> PickColorAsync(Window owner, Color currentColor)
    {
        var picker = new ColorPicker { Color = currentColor, IsAlphaEnabled = false, IsAlphaVisible = false };
        var ok = new Button { Content = "OK" }; var cancel = new Button { Content = "Abbrechen" };
        var dialog = new Window { Title = "Farbe auswählen", Width = 420, Height = 500, CanResize = false, WindowStartupLocation = WindowStartupLocation.CenterOwner };
        ok.Click += (_, _) => dialog.Close(picker.Color); cancel.Click += (_, _) => dialog.Close(null);
        dialog.Content = new StackPanel { Margin = new Thickness(20), Spacing = 12, Children = { picker, new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Spacing = 8, Children = { cancel, ok } } } };
        return await dialog.ShowDialog<Color?>(owner);
    }
}
