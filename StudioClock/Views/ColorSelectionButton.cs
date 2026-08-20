using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace StudioClock.Views;
public sealed class ColorSelectionButton : Button
{
    private readonly Border _swatch;
    private readonly TextBlock _hex;
    private Color _selectedColor;

    public Color SelectedColor
    {
        get => _selectedColor;
        set { _selectedColor = value; _swatch.Background = new SolidColorBrush(value); _hex.Text = ToHex(value); }
    }

    public ColorSelectionButton(Color color)
    {
        HorizontalAlignment = HorizontalAlignment.Stretch; HorizontalContentAlignment = HorizontalAlignment.Left; MinHeight = 38;
        _swatch = new Border { Width = 24, Height = 24, CornerRadius = new CornerRadius(3), BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1) };
        _hex = new TextBlock { VerticalAlignment = VerticalAlignment.Center, FontFamily = FontFamily.Default };
        Content = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10, Children = { _swatch, _hex } };
        SelectedColor = color;
    }

    public static string ToHex(Color color) => $"#{color.R:X2}{color.G:X2}{color.B:X2}";
}
