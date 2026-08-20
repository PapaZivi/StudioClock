using Avalonia.Controls;
using Avalonia.Input;

namespace StudioClock.Services;
public interface IWindowResizeService
{
    void BeginResize(Window window, WindowEdge edge, PointerPressedEventArgs args);
}
