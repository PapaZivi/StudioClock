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
    private readonly LedClockControl _clock;
    private ResizeDirection? _resizeDirection;
    private PixelPoint _resizeStartPosition;
    private PixelPoint _resizeStartPointer;
    private Size _resizeStartSize;
    private double _resizeScaling = 1;
    public AppController? Controller { get; set; }

    public MainWindow(AppSettings settings)
    {
        Title = "StudioClock"; Icon = TrayIconFactory.Create(); Width = settings.WindowWidth; Height = settings.WindowHeight; MinWidth = 200; MinHeight = 200;
        SystemDecorations = SystemDecorations.None; CanResize = true; ShowInTaskbar = false; Background = new SolidColorBrush(Color.Parse(settings.BackgroundColor));
        _clock = new LedClockControl { ClockColor = Color.Parse(settings.ClockColor), LedColor = Color.Parse(settings.LedColor) }; Content = _clock;
        AddHandler(PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel, true);
        AddHandler(PointerMovedEvent, OnPointerMoved, RoutingStrategies.Tunnel, true);
        AddHandler(PointerReleasedEvent, OnPointerReleased, RoutingStrategies.Tunnel, true);
        AddHandler(PointerCaptureLostEvent, OnPointerCaptureLost, RoutingStrategies.Tunnel, true);
        AddHandler(PointerExitedEvent, (_, _) => { if (_resizeDirection is null) Cursor = Cursor.Default; }, RoutingStrategies.Tunnel, true);
        Closing += OnClosing;
    }

    public void Apply(AppSettings settings)
    {
        Topmost = settings.AlwaysOnTop; Opacity = settings.TransparencyEnabled ? settings.OpacityPercent / 100d : 1;
        Background = new SolidColorBrush(Color.Parse(settings.BackgroundColor)); _clock.ClockColor = Color.Parse(settings.ClockColor); _clock.LedColor = Color.Parse(settings.LedColor); _clock.InvalidateVisual();
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e) { if (Controller?.IsExiting != true) { e.Cancel = true; Hide(); Controller?.UpdateMenus(); } }
    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.Source is Visual source && TopLevel.GetTopLevel(source) != this) return;
        var point = e.GetCurrentPoint(this);
        if (point.Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed) { Controller?.OpenContextMenu(this); e.Handled = true; return; }
        if (point.Properties.PointerUpdateKind != PointerUpdateKind.LeftButtonPressed || e.ClickCount > 1) return;
        Controller?.CloseContextMenu();
        var direction = ManualResize.HitTest(e.GetPosition(this), Bounds.Size);
        if (direction is null) { BeginMoveDrag(e); e.Handled = true; return; }
        _resizeDirection = direction; _resizeStartPosition = Position; _resizeStartSize = new Size(Width, Height);
        _resizeStartPointer = this.PointToScreen(e.GetPosition(this)); _resizeScaling = RenderScaling;
        e.Pointer.Capture(this); e.Handled = true;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.Escape && Controller?.IsContextMenuOpen == true) { Controller.CloseContextMenu(); e.Handled = true; return; }
        base.OnKeyDown(e);
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_resizeDirection is not { } direction) { Cursor = CursorForDirection(ManualResize.HitTest(e.GetPosition(this), Bounds.Size)); return; }
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) { EndResize(e.Pointer); return; }
        var result = ManualResize.Calculate(_resizeStartPosition, _resizeStartSize, _resizeStartPointer, this.PointToScreen(e.GetPosition(this)), _resizeScaling, direction);
        Width = result.Size.Width; Height = result.Size.Height; Position = result.Position; e.Handled = true;
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e) { if (_resizeDirection is not null) { EndResize(e.Pointer); e.Handled = true; } }
    private void OnPointerCaptureLost(object? sender, PointerCaptureLostEventArgs e) { if (_resizeDirection is not null) EndResize(null); }
    private void EndResize(IPointer? pointer)
    {
        _resizeDirection = null; if (pointer?.Captured == this) pointer.Capture(null); Controller?.SaveWindowBounds();
    }

    internal static Cursor CursorForDirection(ResizeDirection? direction) => new(direction switch
    {
        ResizeDirection.Left or ResizeDirection.Right => StandardCursorType.SizeWestEast,
        ResizeDirection.Top or ResizeDirection.Bottom => StandardCursorType.SizeNorthSouth,
        ResizeDirection.TopLeft or ResizeDirection.BottomRight => StandardCursorType.TopLeftCorner,
        ResizeDirection.TopRight or ResizeDirection.BottomLeft => StandardCursorType.TopRightCorner,
        _ => StandardCursorType.Arrow
    });
}
