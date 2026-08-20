using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using StudioClock.Controls;
using StudioClock.Helpers;
using StudioClock.Models;
using StudioClock.Services;

namespace StudioClock.Views;
public sealed class MainWindow : Window
{
    internal const double ResizeBorder = 8;
    private readonly LedClockControl _clock;
    private readonly IWindowResizeService _resizeService;
    public AppController? Controller { get; set; }

    public MainWindow(AppSettings settings)
    {
        _resizeService = OperatingSystem.IsWindows() ? new StudioClock.Platform.Windows.WindowsWindowResizeService() : new StudioClock.Platform.MacOS.AvaloniaWindowResizeService();
        Title = "StudioClock"; Icon = TrayIconFactory.Create(); Width = settings.WindowWidth; Height = settings.WindowHeight; MinWidth = 200; MinHeight = 200;
        SystemDecorations = SystemDecorations.None; CanResize = true; ShowInTaskbar = false; Background = new SolidColorBrush(Color.Parse(settings.BackgroundColor));
        _clock = new LedClockControl { ClockColor = Color.Parse(settings.ClockColor), LedColor = Color.Parse(settings.LedColor) }; Content = _clock;
        AddHandler(PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel, true);
        AddHandler(PointerMovedEvent, OnPointerMoved, RoutingStrategies.Tunnel, true);
        AddHandler(PointerExitedEvent, (_, _) => Cursor = Cursor.Default, RoutingStrategies.Tunnel, true);
        Closing += OnClosing;
    }

    public void Apply(AppSettings settings)
    {
        Topmost = settings.AlwaysOnTop; Opacity = settings.TransparencyEnabled ? settings.OpacityPercent / 100d : 1;
        Background = new SolidColorBrush(Color.Parse(settings.BackgroundColor)); _clock.ClockColor = Color.Parse(settings.ClockColor); _clock.LedColor = Color.Parse(settings.LedColor); _clock.InvalidateVisual();
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e) { if (Controller?.IsExiting != true) { e.Cancel = true; Hide(); Controller?.UpdateMenus(); } }
    private void OnPointerMoved(object? sender, PointerEventArgs e) => Cursor = CursorForEdge(GetEdge(e.GetPosition(this)));
    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(this);
        if (point.Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed) { Controller?.OpenContextMenu(this); e.Handled = true; return; }
        if (point.Properties.PointerUpdateKind != PointerUpdateKind.LeftButtonPressed || e.ClickCount > 1) return;
        var edge = GetEdge(e.GetPosition(this));
        if (edge is { } resizeEdge) _resizeService.BeginResize(this, resizeEdge, e); else BeginMoveDrag(e);
        e.Handled = true;
    }

    internal WindowEdge? GetEdge(Point point) => ResizeHitTest.GetEdge(point, Bounds.Size, ResizeBorder);
    internal static Cursor CursorForEdge(WindowEdge? edge) => new(edge switch
    {
        WindowEdge.West or WindowEdge.East => StandardCursorType.SizeWestEast,
        WindowEdge.North or WindowEdge.South => StandardCursorType.SizeNorthSouth,
        WindowEdge.NorthWest or WindowEdge.SouthEast => StandardCursorType.TopLeftCorner,
        WindowEdge.NorthEast or WindowEdge.SouthWest => StandardCursorType.TopRightCorner,
        _ => StandardCursorType.Arrow
    });
}
