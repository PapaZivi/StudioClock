using System.Globalization;using StudioClock.Controls;
namespace StudioClock.Tests;
public sealed class ClockMathTests
{
 [Theory][InlineData(0)][InlineData(1)][InlineData(2)][InlineData(5)][InlineData(15)][InlineData(30)][InlineData(45)][InlineData(59)]
 public void ActiveLedsMatchSecond(int second)
 {
  for(var index=0;index<60;index++)
   Assert.Equal(second==0||index>0&&index<=second,ClockMath.IsSecondLedActive(index,second));
 }
 [Fact]public void EveryFifthLedIsMajor(){for(var i=0;i<60;i++)Assert.Equal(i%5==0,ClockMath.IsMajorLed(i));}
 [Theory][InlineData(300,800,300)][InlineData(800,300,300)][InlineData(1000,600,600)]public void RenderAreaIsSquare(double w,double h,double expected)=>Assert.Equal(expected,ClockMath.SquareSide(w,h));
 [Fact]public void Uses24HourCulture(){var c=(CultureInfo)CultureInfo.InvariantCulture.Clone();c.DateTimeFormat.ShortTimePattern="HH:mm";Assert.Equal("23:07",ClockMath.TimeText(new DateTime(2026,1,1,23,7,0),c));}
 [Fact]public void Uses12HourCulture(){var c=(CultureInfo)CultureInfo.InvariantCulture.Clone();c.DateTimeFormat.ShortTimePattern="h:mm tt";Assert.Equal("11:07",ClockMath.TimeText(new DateTime(2026,1,1,23,7,0),c));}
}
