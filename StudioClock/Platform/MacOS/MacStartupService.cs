using System.Security;
using StudioClock.Services;

namespace StudioClock.Platform.MacOS;

public sealed class MacStartupService : IStartupService
{
    private const string Label = "de.studioclock.app";
    private static string PlistPath => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library", "LaunchAgents", Label + ".plist");
    public bool IsEnabled() => File.Exists(PlistPath);
    public void SetEnabled(bool enabled)
    {
        if (!enabled) { if (File.Exists(PlistPath)) File.Delete(PlistPath); return; }
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(PlistPath)!);
        var exe = SecurityElement.Escape(Environment.ProcessPath ?? throw new InvalidOperationException("Executable path unavailable"));
        File.WriteAllText(PlistPath, $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
            <plist version="1.0"><dict><key>Label</key><string>{Label}</string><key>ProgramArguments</key><array><string>{exe}</string></array><key>RunAtLoad</key><true/></dict></plist>
            """);
    }
}
