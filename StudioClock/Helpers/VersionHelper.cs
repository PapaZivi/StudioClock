namespace StudioClock.Helpers;
public static class VersionHelper
{
    public static string DisplayVersion(string? informationalVersion, string fallback = "–") =>
        string.IsNullOrWhiteSpace(informationalVersion) ? fallback : informationalVersion.Split('+', 2)[0];
}
