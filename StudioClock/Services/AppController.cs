using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using StudioClock.Models;
using StudioClock.Views;
using StudioClock.Helpers;

namespace StudioClock.Services;
public sealed class AppController : IDisposable
{
    private readonly IClassicDesktopStyleApplicationLifetime _desktop;private readonly MainWindow _window;private readonly AppSettings _settings;private readonly SettingsService _settingsService;private readonly IStartupService _startup;
    private TrayIcon? _tray;private NativeMenuItem? _trayVisibility,_trayTopmost,_trayTransparency;public bool IsExiting{get;private set;}
    public AppController(IClassicDesktopStyleApplicationLifetime d,MainWindow w,AppSettings s,SettingsService ss,IStartupService startup){_desktop=d;_window=w;_settings=s;_settingsService=ss;_startup=startup;}
    public void Initialize(){RestoreWindow();_settings.AutoStart=_startup.IsEnabled();_window.Apply(_settings);CreateTray();_window.PositionChanged+=(_,_)=>SaveBounds();_window.PropertyChanged+=(_,e)=>{if(e.Property==Window.ClientSizeProperty)SaveBounds();};_window.Show();}
    public void Show(){if(!_window.IsVisible)_window.Show();_window.WindowState=WindowState.Normal;_window.Activate();UpdateMenus();}
    public void ToggleVisibility(){if(_window.IsVisible)_window.Hide();else Show();UpdateMenus();}
    public void OpenContextMenu(Control owner){var menu=new ContextMenu{ItemsSource=BuildMenu()};menu.Open(owner);}
    public void UpdateMenus(){if(_trayVisibility!=null)_trayVisibility.Header=_window.IsVisible?"Verstecken":"Anzeigen";if(_trayTopmost!=null)_trayTopmost.IsChecked=_settings.AlwaysOnTop;if(_trayTransparency!=null)_trayTransparency.IsChecked=_settings.TransparencyEnabled;}
    private List<Control> BuildMenu()
    {
        var top=new MenuItem{Header="Always on top",ToggleType=MenuItemToggleType.CheckBox,IsChecked=_settings.AlwaysOnTop};top.Click+=(_,_)=>ToggleTopmost();var trans=new MenuItem{Header="Transparenz aktivieren",ToggleType=MenuItemToggleType.CheckBox,IsChecked=_settings.TransparencyEnabled};trans.Click+=(_,_)=>ToggleTransparency();
        return[top,trans,Item("Verstecken",ToggleVisibility),Item("Einstellungen",async()=>await ShowSettings()),Item("About",async()=>await new AboutWindow().ShowDialog(_window)),new Separator(),Item("Beenden",Exit)];
    }
    private static MenuItem Item(string h,Action a){var i=new MenuItem{Header=h};i.Click+=(_,_)=>a();return i;}
    private void CreateTray(){_trayTopmost=NativeItem("Always on top",ToggleTopmost,true);_trayTransparency=NativeItem("Transparenz aktivieren",ToggleTransparency,true);_trayVisibility=NativeItem("Verstecken",ToggleVisibility);var m=new NativeMenu();m.Items.Add(_trayTopmost);m.Items.Add(_trayTransparency);m.Items.Add(_trayVisibility);m.Items.Add(NativeItem("Einstellungen",async()=>await ShowSettings()));m.Items.Add(NativeItem("About",async()=>await new AboutWindow().ShowDialog(_window)));m.Items.Add(new NativeMenuItemSeparator());m.Items.Add(NativeItem("Beenden",Exit));_tray=new TrayIcon{Icon=TrayIconFactory.Create(),ToolTipText="StudioClock",Menu=m,IsVisible=true};_tray.Clicked+=(_,_)=>ToggleVisibility();UpdateMenus();}
    private static NativeMenuItem NativeItem(string h,Action a,bool check=false){var i=new NativeMenuItem(h);if(check)i.ToggleType=NativeMenuItemToggleType.CheckBox;i.Click+=(_,_)=>a();return i;}
    private void ToggleTopmost(){_settings.AlwaysOnTop=!_settings.AlwaysOnTop;ApplyAndSave();}private void ToggleTransparency(){_settings.TransparencyEnabled=!_settings.TransparencyEnabled;ApplyAndSave();}
    private async Task ShowSettings(){Show();if(await new SettingsWindow(_settings,_startup).ShowDialog<bool>(_window))ApplyAndSave();}private void ApplyAndSave(){_window.Apply(_settings);_settingsService.Save(_settings);UpdateMenus();}
    private void RestoreWindow(){_window.Width=_settings.WindowWidth;_window.Height=_settings.WindowHeight;if(_settings.WindowX is{}x&&_settings.WindowY is{}y){var p=new PixelPoint((int)x,(int)y);var s=_window.Screens.ScreenFromPoint(p);if(s!=null)_window.Position=ClampToScreen(p,s.WorkingArea,(int)_settings.WindowWidth,(int)_settings.WindowHeight);else _window.WindowStartupLocation=WindowStartupLocation.CenterScreen;}else _window.WindowStartupLocation=WindowStartupLocation.CenterScreen;}
    public static PixelPoint ClampToScreen(PixelPoint p,PixelRect a,int w,int h)=>new(Math.Clamp(p.X,a.X-w+80,a.Right-80),Math.Clamp(p.Y,a.Y,a.Bottom-80));
    private void SaveBounds(){if(_window.WindowState!=WindowState.Normal)return;_settings.WindowX=_window.Position.X;_settings.WindowY=_window.Position.Y;_settings.WindowWidth=_window.Width;_settings.WindowHeight=_window.Height;_settingsService.Save(_settings);}
    private void Exit(){IsExiting=true;SaveBounds();_tray?.Dispose();_desktop.Shutdown();}public void Dispose(){_tray?.Dispose();App.SingleInstance?.Dispose();}
}
