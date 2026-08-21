using Avalonia.Controls;
using Avalonia.Platform;

namespace StudioClock.Helpers;

public static class TrayIconFactory
{
    public static WindowIcon Create()
    {
        using var icon = AssetLoader.Open(new Uri("avares://StudioClock/Assets/StudioClock.ico"));
        return new WindowIcon(icon);
    }
}
