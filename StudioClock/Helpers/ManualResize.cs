using Avalonia;

namespace StudioClock.Helpers;

public enum ResizeDirection
{
    Left, Right, Top, Bottom, TopLeft, TopRight, BottomLeft, BottomRight
}

public readonly record struct ResizeResult(PixelPoint Position, Size Size);

public static class ManualResize
{
    public static ResizeDirection? HitTest(Point point, Size size, double edgeZone = 8, double cornerZone = 14)
    {
        var cornerLeft = point.X >= 0 && point.X <= cornerZone;
        var cornerRight = point.X <= size.Width && point.X >= size.Width - cornerZone;
        var cornerTop = point.Y >= 0 && point.Y <= cornerZone;
        var cornerBottom = point.Y <= size.Height && point.Y >= size.Height - cornerZone;
        if (cornerTop && cornerLeft) return ResizeDirection.TopLeft;
        if (cornerTop && cornerRight) return ResizeDirection.TopRight;
        if (cornerBottom && cornerLeft) return ResizeDirection.BottomLeft;
        if (cornerBottom && cornerRight) return ResizeDirection.BottomRight;

        var left = point.X >= 0 && point.X <= edgeZone; var right = point.X <= size.Width && point.X >= size.Width - edgeZone;
        var top = point.Y >= 0 && point.Y <= edgeZone; var bottom = point.Y <= size.Height && point.Y >= size.Height - edgeZone;
        if (left) return ResizeDirection.Left; if (right) return ResizeDirection.Right;
        if (top) return ResizeDirection.Top; if (bottom) return ResizeDirection.Bottom; return null;
    }

    public static ResizeResult Calculate(PixelPoint startPosition, Size startSize, PixelPoint startPointer, PixelPoint currentPointer,
        double renderScaling, ResizeDirection direction, double minimum = 200)
    {
        if (!double.IsFinite(renderScaling) || renderScaling <= 0) renderScaling = 1;
        var deltaX = (currentPointer.X - startPointer.X) / renderScaling;
        var deltaY = (currentPointer.Y - startPointer.Y) / renderScaling;
        var left = direction is ResizeDirection.Left or ResizeDirection.TopLeft or ResizeDirection.BottomLeft;
        var right = direction is ResizeDirection.Right or ResizeDirection.TopRight or ResizeDirection.BottomRight;
        var top = direction is ResizeDirection.Top or ResizeDirection.TopLeft or ResizeDirection.TopRight;
        var bottom = direction is ResizeDirection.Bottom or ResizeDirection.BottomLeft or ResizeDirection.BottomRight;
        var width = left ? Math.Max(minimum, startSize.Width - deltaX) : right ? Math.Max(minimum, startSize.Width + deltaX) : startSize.Width;
        var height = top ? Math.Max(minimum, startSize.Height - deltaY) : bottom ? Math.Max(minimum, startSize.Height + deltaY) : startSize.Height;
        var x = left ? startPosition.X + (int)Math.Round((startSize.Width - width) * renderScaling) : startPosition.X;
        var y = top ? startPosition.Y + (int)Math.Round((startSize.Height - height) * renderScaling) : startPosition.Y;
        return new ResizeResult(new PixelPoint(x, y), new Size(width, height));
    }
}
