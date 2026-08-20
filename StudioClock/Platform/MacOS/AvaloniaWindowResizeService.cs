using Avalonia.Controls;
using Avalonia.Input;
using StudioClock.Services;

namespace StudioClock.Platform.MacOS;
public sealed class AvaloniaWindowResizeService : IWindowResizeService
{
    public void BeginResize(Window window, WindowEdge edge, PointerPressedEventArgs args) => window.BeginResizeDrag(edge, args);
}
