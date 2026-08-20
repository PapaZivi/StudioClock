using System.Globalization;

namespace StudioClock.Controls;

public static class ClockMath
{
    public static int ActiveLedCount(int second) => Math.Clamp(second, 0, 59);
    public static bool IsMajorLed(int index) => index is >= 0 and < 60 && index % 5 == 0;
    public static double SquareSide(double width, double height) => Math.Max(0, Math.Min(width, height));
    public static string TimeText(DateTime time, CultureInfo culture)
    {
        var pattern = culture.DateTimeFormat.ShortTimePattern;
        var is24Hour = pattern.Contains('H');
        return time.ToString(is24Hour ? "HH:mm" : "hh:mm", culture);
    }
}

