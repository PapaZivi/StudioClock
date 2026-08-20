using StudioClock.Services;
namespace StudioClock.Tests;
public sealed class SettingsServiceTests:IDisposable
{
 private readonly string _dir=Path.Combine(Path.GetTempPath(),"StudioClockTests-"+Guid.NewGuid());private string FilePath=>Path.Combine(_dir,"settings.json");
 [Fact]public void MissingFileReturnsDefaults(){var s=new SettingsService(FilePath).Load();Assert.Equal("#AA0000",s.ClockColor);Assert.Equal(500,s.WindowWidth);}
 [Fact]public void BrokenJsonReturnsDefaults(){Directory.CreateDirectory(_dir);File.WriteAllText(FilePath,"{broken");var s=new SettingsService(FilePath).Load();Assert.Equal("#000000",s.BackgroundColor);}
 [Fact]public void InvalidValuesAreNormalized(){Directory.CreateDirectory(_dir);File.WriteAllText(FilePath,"""{"WindowWidth":-1,"OpacityPercent":999,"ClockColor":"nope"}""");var s=new SettingsService(FilePath).Load();Assert.Equal(500,s.WindowWidth);Assert.Equal(100,s.OpacityPercent);Assert.Equal("#AA0000",s.ClockColor);}
 [Fact]public void RoundTrips(){var service=new SettingsService(FilePath);var s=service.Load();s.LedColor="#123456";s.OpacityPercent=42;service.Save(s);var loaded=service.Load();Assert.Equal("#123456",loaded.LedColor);Assert.Equal(42,loaded.OpacityPercent);}
 [Fact]public void ExistingJsonShapeRemainsCompatible(){Directory.CreateDirectory(_dir);File.WriteAllText(FilePath,"""{"WindowX":12,"WindowY":34,"WindowWidth":640,"WindowHeight":480,"AlwaysOnTop":true,"TransparencyEnabled":true,"OpacityPercent":55,"ClockColor":"#112233","BackgroundColor":"#445566","LedColor":"#778899","AutoStart":true}""");var loaded=new SettingsService(FilePath).Load();Assert.Equal(12,loaded.WindowX);Assert.Equal(34,loaded.WindowY);Assert.Equal(640,loaded.WindowWidth);Assert.Equal("#112233",loaded.ClockColor);Assert.True(loaded.AutoStart);}
 [Fact]public void SaveFailureIsReported(){var invalidPath=string.Concat("invalid",(char)0,"path",Path.DirectorySeparatorChar,"settings.json");var service=new SettingsService(invalidPath);Assert.False(service.Save(new()));Assert.NotNull(service.LastSaveError);}
 public void Dispose(){if(Directory.Exists(_dir))Directory.Delete(_dir,true);}
}
