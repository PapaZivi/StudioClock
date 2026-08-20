using Avalonia;using StudioClock.Services;
namespace StudioClock.Tests;
public sealed class WindowPlacementTests
{
 [Fact]public void KeepsAtLeastPartOfWindowVisible(){var p=AppController.ClampToScreen(new PixelPoint(9000,9000),new PixelRect(0,0,1920,1080),500,500);Assert.Equal(1840,p.X);Assert.Equal(1000,p.Y);}
 [Fact]public void PreservesValidPosition(){var p=AppController.ClampToScreen(new PixelPoint(100,100),new PixelRect(0,0,1920,1080),500,500);Assert.Equal(new PixelPoint(100,100),p);}
}
