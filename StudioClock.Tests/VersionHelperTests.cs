using StudioClock.Helpers;
namespace StudioClock.Tests;
public sealed class VersionHelperTests
{
    [Theory] [InlineData("202608.1", "202608.1")] [InlineData("202608.1+abcdef", "202608.1")]
    public void RemovesBuildMetadata(string input, string expected) => Assert.Equal(expected, VersionHelper.DisplayVersion(input));

    [Fact] public void CurrentReleaseVersionIsExpected() => Assert.Equal("2026.08.3", VersionHelper.DisplayVersion("2026.08.3+ignored"));
}
