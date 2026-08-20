using Avalonia.Media;
using StudioClock.Models;

namespace StudioClock.Helpers;

public static class SettingsValidator
{
    public static AppSettings Normalize(AppSettings? value)
    {
        value ??= new AppSettings();
        value.WindowWidth = ValidSize(value.WindowWidth) ? value.WindowWidth : 500;
        value.WindowHeight = ValidSize(value.WindowHeight) ? value.WindowHeight : 500;
        value.WindowX = ValidCoordinate(value.WindowX) ? value.WindowX : null;
        value.WindowY = ValidCoordinate(value.WindowY) ? value.WindowY : null;
        value.OpacityPercent = Math.Clamp(value.OpacityPercent, 10, 100);
        value.ClockColor = ValidColor(value.ClockColor, "#AA0000");
        value.BackgroundColor = ValidColor(value.BackgroundColor, "#000000");
        value.LedColor = ValidColor(value.LedColor, "#AA0000");
        return value;
    }

    private static bool ValidSize(double v) => double.IsFinite(v) && v >= 200 && v <= 10000;
    private static bool ValidCoordinate(double? v) => v is { } n && double.IsFinite(n) && Math.Abs(n) < 1_000_000;
    private static string ValidColor(string? value, string fallback) =>
        Color.TryParse(value, out _) ? value! : fallback;
}

