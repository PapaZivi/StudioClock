using Avalonia;
using StudioClock.Helpers;

namespace StudioClock.Tests;
public sealed class ResizeHitTestTests
{
    private static readonly Size WindowSize = new(500, 400);
    [Theory]
    [InlineData(0, 0, ResizeDirection.TopLeft)] [InlineData(499, 0, ResizeDirection.TopRight)]
    [InlineData(0, 399, ResizeDirection.BottomLeft)] [InlineData(499, 399, ResizeDirection.BottomRight)]
    [InlineData(0, 200, ResizeDirection.Left)] [InlineData(499, 200, ResizeDirection.Right)]
    [InlineData(250, 0, ResizeDirection.Top)] [InlineData(250, 399, ResizeDirection.Bottom)]
    public void DetectsEveryResizeZone(double x, double y, ResizeDirection expected) => Assert.Equal(expected, ManualResize.HitTest(new Point(x, y), WindowSize));

    [Fact] public void InteriorRemainsAvailableForDragging() => Assert.Null(ManualResize.HitTest(new Point(250, 200), WindowSize));

    [Theory]
    [InlineData(ResizeDirection.Right, 100, 0, 600, 500, 1000, 1000)]
    [InlineData(ResizeDirection.Bottom, 0, 100, 500, 600, 1000, 1000)]
    [InlineData(ResizeDirection.Left, -100, 0, 600, 500, 900, 1000)]
    [InlineData(ResizeDirection.Top, 0, -100, 500, 600, 1000, 900)]
    [InlineData(ResizeDirection.BottomRight, 100, 50, 600, 550, 1000, 1000)]
    public void CalculatesRequestedResize(ResizeDirection direction, int dx, int dy, double width, double height, int x, int y)
    {
        var result = ManualResize.Calculate(new PixelPoint(1000, 1000), new Size(500, 500), new PixelPoint(2000, 2000), new PixelPoint(2000 + dx, 2000 + dy), 1, direction);
        Assert.Equal(new Size(width, height), result.Size); Assert.Equal(new PixelPoint(x, y), result.Position);
    }

    [Theory] [InlineData(1.25)] [InlineData(1.5)] [InlineData(1.75)] [InlineData(2.0)]
    public void ConvertsPhysicalPixelDeltaToDips(double scaling)
    {
        var pixels = (int)Math.Round(100 * scaling);
        var result = ManualResize.Calculate(new PixelPoint(100, 100), new Size(500, 500), new PixelPoint(0, 0), new PixelPoint(pixels, 0), scaling, ResizeDirection.Right);
        Assert.Equal(600, result.Size.Width, 6);
    }

    [Fact] public void MinimumSizeKeepsOppositeEdgesFixed()
    {
        var result = ManualResize.Calculate(new PixelPoint(1000, 1000), new Size(500, 500), new PixelPoint(0, 0), new PixelPoint(1000, 1000), 1.5, ResizeDirection.TopLeft);
        Assert.Equal(new Size(200, 200), result.Size); Assert.Equal(new PixelPoint(1450, 1450), result.Position);
    }
}
