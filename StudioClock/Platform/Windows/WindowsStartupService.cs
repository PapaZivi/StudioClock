using Microsoft.Win32;
using System.Runtime.Versioning;
using StudioClock.Services;

namespace StudioClock.Platform.Windows;

[SupportedOSPlatform("windows")]
public sealed class WindowsStartupService : IStartupService
{
    private const string KeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    public bool IsEnabled()
    {
        try { using var key = Registry.CurrentUser.OpenSubKey(KeyPath); return key?.GetValue("StudioClock") is string; }
        catch { return false; }
    }
    public void SetEnabled(bool enabled)
    {
        using var key = Registry.CurrentUser.CreateSubKey(KeyPath);
        if (enabled) key.SetValue("StudioClock", $"\"{Environment.ProcessPath}\"");
        else key.DeleteValue("StudioClock", false);
    }
}
