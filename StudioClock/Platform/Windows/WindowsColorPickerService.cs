using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Avalonia.Controls;
using Avalonia.Media;
using StudioClock.Services;

namespace StudioClock.Platform.Windows;

[SupportedOSPlatform("windows")]
public sealed class WindowsColorPickerService : IColorPickerService
{
    private const uint CcRgbInit = 0x1;
    private const uint CcFullOpen = 0x2;

    public Task<Color?> PickColorAsync(Window owner, Color currentColor)
    {
        var customColors = Marshal.AllocHGlobal(16 * sizeof(uint));
        try
        {
            for (var i = 0; i < 16; i++) Marshal.WriteInt32(customColors, i * sizeof(uint), 0x00FFFFFF);
            var chooseColor = new ChooseColor
            {
                StructSize = (uint)Marshal.SizeOf<ChooseColor>(),
                Owner = owner.TryGetPlatformHandle()?.Handle ?? 0,
                Result = ToColorRef(currentColor),
                CustomColors = customColors,
                Flags = CcRgbInit | CcFullOpen
            };
            if (!ChooseColorW(ref chooseColor)) return Task.FromResult<Color?>(null);
            return Task.FromResult<Color?>(FromColorRef(chooseColor.Result));
        }
        finally { Marshal.FreeHGlobal(customColors); }
    }

    internal static uint ToColorRef(Color color) => (uint)(color.R | color.G << 8 | color.B << 16);
    internal static Color FromColorRef(uint value) => Color.FromRgb((byte)value, (byte)(value >> 8), (byte)(value >> 16));

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct ChooseColor
    {
        public uint StructSize; public nint Owner; public nint Instance; public uint Result; public nint CustomColors;
        public uint Flags; public nint CustomData; public nint Hook; public nint TemplateName;
    }

    [DllImport("comdlg32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ChooseColorW(ref ChooseColor chooseColor);
}
