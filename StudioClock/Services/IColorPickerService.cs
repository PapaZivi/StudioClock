using Avalonia.Controls;
using Avalonia.Media;

namespace StudioClock.Services;
public interface IColorPickerService
{
    Task<Color?> PickColorAsync(Window owner, Color currentColor);
}
