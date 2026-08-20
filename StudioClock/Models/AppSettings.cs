namespace StudioClock.Models;

public sealed class AppSettings
{
    public double? WindowX { get; set; }
    public double? WindowY { get; set; }
    public double WindowWidth { get; set; } = 500;
    public double WindowHeight { get; set; } = 500;
    public bool AlwaysOnTop { get; set; }
    public bool TransparencyEnabled { get; set; }
    public int OpacityPercent { get; set; } = 100;
    public string ClockColor { get; set; } = "#AA0000";
    public string BackgroundColor { get; set; } = "#000000";
    public string LedColor { get; set; } = "#AA0000";
    public bool AutoStart { get; set; }
}

