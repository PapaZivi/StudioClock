using Avalonia;
using Avalonia.Controls;
using StudioClock.Helpers;

namespace StudioClock.Tests;
public sealed class ResizeHitTestTests
{
    private static readonly Size Size = new(500, 400);
    [Theory]
    [InlineData(0, 0, WindowEdge.NorthWest)] [InlineData(499, 0, WindowEdge.NorthEast)]
    [InlineData(0, 399, WindowEdge.SouthWest)] [InlineData(499, 399, WindowEdge.SouthEast)]
    [InlineData(0, 200, WindowEdge.West)] [InlineData(499, 200, WindowEdge.East)]
    [InlineData(250, 0, WindowEdge.North)] [InlineData(250, 399, WindowEdge.South)]
    public void DetectsEveryResizeZone(double x, double y, WindowEdge expected) => Assert.Equal(expected, ResizeHitTest.GetEdge(new Point(x, y), Size, 8));

    [Fact] public void InteriorRemainsAvailableForDragging() => Assert.Null(ResizeHitTest.GetEdge(new Point(250, 200), Size, 8));
}
