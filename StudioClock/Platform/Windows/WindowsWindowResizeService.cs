using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Avalonia.Controls;
using Avalonia.Input;
using StudioClock.Services;

namespace StudioClock.Platform.Windows;

[SupportedOSPlatform("windows")]
public sealed class WindowsWindowResizeService : IWindowResizeService
{
    private const uint WmNcLButtonDown = 0x00A1;
    public void BeginResize(Window window, WindowEdge edge, PointerPressedEventArgs args)
    {
        var handle = window.TryGetPlatformHandle()?.Handle ?? 0;
        if (handle == 0) { window.BeginResizeDrag(edge, args); return; }
        ReleaseCapture();
        SendMessageW(handle, WmNcLButtonDown, HitTest(edge), 0);
    }

    private static nint HitTest(WindowEdge edge) => edge switch
    {
        WindowEdge.West => 10, WindowEdge.East => 11, WindowEdge.North => 12, WindowEdge.NorthWest => 13,
        WindowEdge.NorthEast => 14, WindowEdge.South => 15, WindowEdge.SouthWest => 16, WindowEdge.SouthEast => 17, _ => 0
    };
    [DllImport("user32.dll")] [return: MarshalAs(UnmanagedType.Bool)] private static extern bool ReleaseCapture();
    [DllImport("user32.dll", CharSet = CharSet.Unicode)] private static extern nint SendMessageW(nint window, uint message, nint wParam, nint lParam);
}
