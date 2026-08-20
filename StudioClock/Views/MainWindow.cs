using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using StudioClock.Controls;
using StudioClock.Models;
using StudioClock.Services;
using StudioClock.Helpers;

namespace StudioClock.Views;
public sealed class MainWindow : Window
{
    private const double ResizeBorder = 7; private readonly LedClockControl _clock; public AppController? Controller { get; set; }
    public MainWindow(AppSettings settings)
    {
        Title="StudioClock"; Icon=TrayIconFactory.Create(); Width=settings.WindowWidth; Height=settings.WindowHeight; MinWidth=200; MinHeight=200; SystemDecorations=SystemDecorations.None; CanResize=true; ShowInTaskbar=false;
        ExtendClientAreaToDecorationsHint=true; ExtendClientAreaChromeHints=Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome; Background=new SolidColorBrush(Color.Parse(settings.BackgroundColor));
        _clock=new LedClockControl { ClockColor=Color.Parse(settings.ClockColor), LedColor=Color.Parse(settings.LedColor) }; Content=_clock; PointerPressed+=OnPointerPressed; Closing+=OnClosing;
    }
    public void Apply(AppSettings s) { Topmost=s.AlwaysOnTop; Opacity=s.TransparencyEnabled?s.OpacityPercent/100d:1; Background=new SolidColorBrush(Color.Parse(s.BackgroundColor)); _clock.ClockColor=Color.Parse(s.ClockColor); _clock.LedColor=Color.Parse(s.LedColor); _clock.InvalidateVisual(); }
    private void OnClosing(object? sender, WindowClosingEventArgs e) { if (Controller?.IsExiting!=true) { e.Cancel=true; Hide(); Controller?.UpdateMenus(); } }
    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var p=e.GetCurrentPoint(this); if(p.Properties.PointerUpdateKind==PointerUpdateKind.RightButtonPressed){Controller?.OpenContextMenu(this);e.Handled=true;return;}
        if(p.Properties.PointerUpdateKind!=PointerUpdateKind.LeftButtonPressed||e.ClickCount>1)return; var edge=GetEdge(e.GetPosition(this)); if(edge is {} value)BeginResizeDrag(value,e);else BeginMoveDrag(e);e.Handled=true;
    }
    private WindowEdge? GetEdge(Point p)
    {
        var l=p.X<=ResizeBorder;var r=p.X>=Bounds.Width-ResizeBorder;var t=p.Y<=ResizeBorder;var b=p.Y>=Bounds.Height-ResizeBorder;
        if(t&&l)return WindowEdge.NorthWest;if(t&&r)return WindowEdge.NorthEast;if(b&&l)return WindowEdge.SouthWest;if(b&&r)return WindowEdge.SouthEast;if(l)return WindowEdge.West;if(r)return WindowEdge.East;if(t)return WindowEdge.North;if(b)return WindowEdge.South;return null;
    }
}
