using Avalonia;
using Avalonia.Controls;

namespace StudioClock.Helpers;
public static class ResizeHitTest
{
    public static WindowEdge? GetEdge(Point point, Size size, double border)
    {
        var left = point.X >= 0 && point.X <= border; var right = point.X <= size.Width && point.X >= size.Width - border;
        var top = point.Y >= 0 && point.Y <= border; var bottom = point.Y <= size.Height && point.Y >= size.Height - border;
        if (top && left) return WindowEdge.NorthWest; if (top && right) return WindowEdge.NorthEast;
        if (bottom && left) return WindowEdge.SouthWest; if (bottom && right) return WindowEdge.SouthEast;
        if (left) return WindowEdge.West; if (right) return WindowEdge.East; if (top) return WindowEdge.North; if (bottom) return WindowEdge.South; return null;
    }
}
