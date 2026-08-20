using StudioClock.Services;
namespace StudioClock.Tests;
public sealed class SettingsServiceTests:IDisposable
{
 private readonly string _dir=Path.Combine(Path.GetTempPath(),"StudioClockTests-"+Guid.NewGuid());private string FilePath=>Path.Combine(_dir,"settings.json");
 [Fact]public void MissingFileReturnsDefaults(){var s=new SettingsService(FilePath).Load();Assert.Equal("#AA0000",s.ClockColor);Assert.Equal(500,s.WindowWidth);}
 [Fact]public void BrokenJsonReturnsDefaults(){Directory.CreateDirectory(_dir);File.WriteAllText(FilePath,"{broken");var s=new SettingsService(FilePath).Load();Assert.Equal("#000000",s.BackgroundColor);}
 [Fact]public void InvalidValuesAreNormalized(){Directory.CreateDirectory(_dir);File.WriteAllText(FilePath,"""{"WindowWidth":-1,"OpacityPercent":999,"ClockColor":"nope"}""");var s=new SettingsService(FilePath).Load();Assert.Equal(500,s.WindowWidth);Assert.Equal(100,s.OpacityPercent);Assert.Equal("#AA0000",s.ClockColor);}
 [Fact]public void RoundTrips(){var service=new SettingsService(FilePath);var s=service.Load();s.LedColor="#123456";s.OpacityPercent=42;service.Save(s);var loaded=service.Load();Assert.Equal("#123456",loaded.LedColor);Assert.Equal(42,loaded.OpacityPercent);}
 public void Dispose(){if(Directory.Exists(_dir))Directory.Delete(_dir,true);}
}
