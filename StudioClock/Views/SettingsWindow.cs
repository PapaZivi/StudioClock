using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using StudioClock.Models;
using StudioClock.Services;

namespace StudioClock.Views;
public sealed class SettingsWindow : Window
{
    public SettingsWindow(AppSettings settings, IStartupService startup, IColorPickerService colorPicker)
    {
        Title = "StudioClock – Einstellungen"; Width = 480; Height = 390; CanResize = false; WindowStartupLocation = WindowStartupLocation.CenterOwner;
        var clock = new ColorSelectionButton(Color.Parse(settings.ClockColor));
        var background = new ColorSelectionButton(Color.Parse(settings.BackgroundColor));
        var led = new ColorSelectionButton(Color.Parse(settings.LedColor));
        ConnectPicker(clock, colorPicker); ConnectPicker(background, colorPicker); ConnectPicker(led, colorPicker);

        var slider = new Slider { Minimum = 10, Maximum = 100, Value = settings.OpacityPercent, TickFrequency = 1, HorizontalAlignment = HorizontalAlignment.Stretch };
        var percent = new TextBlock { Text = $"{settings.OpacityPercent}%", VerticalAlignment = VerticalAlignment.Center, Width = 48, TextAlignment = TextAlignment.Right };
        slider.PropertyChanged += (_, e) => { if (e.Property == Slider.ValueProperty) percent.Text = $"{(int)Math.Round(slider.Value)}%"; };
        var autoStart = new CheckBox { Content = "Mit dem System starten", IsChecked = settings.AutoStart };
        var save = new Button { Content = "Speichern" }; var cancel = new Button { Content = "Abbrechen" };
        save.Click += (_, _) =>
        {
            settings.ClockColor = ColorSelectionButton.ToHex(clock.SelectedColor); settings.BackgroundColor = ColorSelectionButton.ToHex(background.SelectedColor);
            settings.LedColor = ColorSelectionButton.ToHex(led.SelectedColor); settings.OpacityPercent = Math.Clamp((int)Math.Round(slider.Value), 10, 100);
            var desired = autoStart.IsChecked == true;
            try { startup.SetEnabled(desired); settings.AutoStart = desired; } catch { settings.AutoStart = startup.IsEnabled(); }
            Close(true);
        };
        cancel.Click += (_, _) => Close(false);

        var grid = new Grid { ColumnDefinitions = new ColumnDefinitions("160,*"), RowDefinitions = new RowDefinitions("Auto,Auto,Auto,18,Auto,Auto,20,Auto"), RowSpacing = 10, ColumnSpacing = 12 };
        AddRow(grid, 0, "Uhrfarbe", clock); AddRow(grid, 1, "Hintergrundfarbe", background); AddRow(grid, 2, "LED-Kranzfarbe", led);
        var transparencyLabel = Label("Transparenz"); Grid.SetRow(transparencyLabel, 4); Grid.SetColumnSpan(transparencyLabel, 2); grid.Children.Add(transparencyLabel);
        var sliderRow = new Grid { ColumnDefinitions = new ColumnDefinitions("*,52") }; Grid.SetRow(slider, 0); Grid.SetColumn(percent, 1); sliderRow.Children.Add(slider); sliderRow.Children.Add(percent);
        Grid.SetRow(sliderRow, 5); Grid.SetColumnSpan(sliderRow, 2); grid.Children.Add(sliderRow);
        Grid.SetRow(autoStart, 7); Grid.SetColumnSpan(autoStart, 2); grid.Children.Add(autoStart);
        Content = new DockPanel { Margin = new Thickness(24), LastChildFill = true, Children =
        {
            new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Spacing = 8, Margin = new Thickness(0,16,0,0), Children = { cancel, save }, [DockPanel.DockProperty] = Dock.Bottom }, grid
        }};
    }

    private async void PickColor(ColorSelectionButton button, IColorPickerService picker)
    {
        var selected = await picker.PickColorAsync(this, button.SelectedColor);
        if (selected is { } color) button.SelectedColor = color;
    }
    private void ConnectPicker(ColorSelectionButton button, IColorPickerService picker) => button.Click += (_, _) => PickColor(button, picker);
    private static TextBlock Label(string text) => new() { Text = text, FontWeight = FontWeight.SemiBold, VerticalAlignment = VerticalAlignment.Center };
    private static void AddRow(Grid grid, int row, string text, Control control)
    {
        var label = Label(text); Grid.SetRow(label, row); Grid.SetRow(control, row); Grid.SetColumn(control, 1); grid.Children.Add(label); grid.Children.Add(control);
    }
}
