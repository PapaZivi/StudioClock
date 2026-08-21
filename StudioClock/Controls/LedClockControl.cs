using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace StudioClock.Controls;

public sealed class LedClockControl : Control
{
    public static readonly StyledProperty<Color> ClockColorProperty = AvaloniaProperty.Register<LedClockControl, Color>(nameof(ClockColor), Color.Parse("#AA0000"));
    public static readonly StyledProperty<Color> LedColorProperty = AvaloniaProperty.Register<LedClockControl, Color>(nameof(LedColor), Color.Parse("#AA0000"));
    private readonly DispatcherTimer _timer;
    private DateTime _now = DateTime.Now;
    public Color ClockColor { get => GetValue(ClockColorProperty); set => SetValue(ClockColorProperty, value); }
    public Color LedColor { get => GetValue(LedColorProperty); set => SetValue(LedColorProperty, value); }

    private static readonly string[] Glyphs =
    [
        "111111101101101101111", "010110010010010010111", "111001001111100100111", "111001001111001001111", "101101101111001001001",
        "111100100111001001111", "111100100111101101111", "111001001010010010010", "111101101111101101111", "111101101111001001111"
    ];

    public LedClockControl()
    {
        ClipToBounds = true;
        _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(250), DispatcherPriority.Background, (_, _) => { _now = DateTime.Now; InvalidateVisual(); });
        _timer.Start();
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        var side = ClockMath.SquareSide(Bounds.Width, Bounds.Height);
        if (side <= 0) return;
        var origin = new Point((Bounds.Width - side) / 2, (Bounds.Height - side) / 2);
        var center = origin + new Vector(side / 2, side / 2);
        var ringRadius = side * .445;
        var normal = Math.Max(1.2, side * .008);
        var major = normal * 1.65;
        var activeBrush = new SolidColorBrush(LedColor);
        var inactiveBrush = new SolidColorBrush(Color.FromArgb(55, LedColor.R, LedColor.G, LedColor.B));
        for (var i = 0; i < 60; i++)
        {
            var angle = i * Math.PI / 30 - Math.PI / 2;
            var p = new Point(center.X + Math.Cos(angle) * ringRadius, center.Y + Math.Sin(angle) * ringRadius);
            var radius = ClockMath.IsMajorLed(i) ? major : normal;
            context.DrawEllipse(ClockMath.IsSecondLedActive(i, _now.Second) ? activeBrush : inactiveBrush, null, p, radius, radius);
        }

        DrawTime(context, ClockMath.TimeText(_now, CultureInfo.CurrentCulture), center, side);
    }

    private void DrawTime(DrawingContext context, string text, Point center, double side)
    {
        const int digitColumns = 3;
        var dot = side * .013;
        var gap = dot * .65;
        var digitWidth = digitColumns * dot * 2 + (digitColumns - 1) * gap;
        var colonWidth = dot * 2;
        var total = digitWidth * 4 + colonWidth + gap * 6;
        var x = center.X - total / 2;
        var y = center.Y - (7 * dot * 2 + 6 * gap) / 2;
        var brush = new SolidColorBrush(ClockColor);
        foreach (var ch in text)
        {
            if (ch == ':')
            {
                context.DrawEllipse(brush, null, new Point(x + dot, center.Y - dot * 2.1), dot, dot);
                context.DrawEllipse(brush, null, new Point(x + dot, center.Y + dot * 2.1), dot, dot);
                x += colonWidth + gap * 2;
                continue;
            }
            var glyph = Glyphs[ch - '0'];
            for (var row = 0; row < 7; row++)
            for (var col = 0; col < 3; col++)
                if (glyph[row * 3 + col] == '1')
                    context.DrawEllipse(brush, null, new Point(x + dot + col * (dot * 2 + gap), y + dot + row * (dot * 2 + gap)), dot, dot);
            x += digitWidth + gap * 2;
        }
    }
}
