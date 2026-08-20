# StudioClock

StudioClock ist eine minimalistische, randlose LED-Desktopuhr für Windows und macOS. Ziffern und der Sekundenkranz werden vollständig mathematisch durch ein eigenes Avalonia-Control gezeichnet; es werden keine fremden Fonts, Grafiken oder Uhr-Assets verwendet.

> Screenshot-Platzhalter: `docs/screenshot.png`

## Voraussetzungen und Build

- .NET SDK 10
- Windows 10/11 oder eine aktuelle macOS-Version

```powershell
dotnet restore
dotnet build
dotnet test
```

Self-contained Builds:

```powershell
dotnet publish StudioClock/StudioClock.csproj -c Release -r win-x64 --self-contained true
dotnet publish StudioClock/StudioClock.csproj -c Release -r win-arm64 --self-contained true
dotnet publish StudioClock/StudioClock.csproj -c Release -r osx-x64 --self-contained true
dotnet publish StudioClock/StudioClock.csproj -c Release -r osx-arm64 --self-contained true
```

Windows erzeugt `StudioClock.exe` ohne Konsolenfenster. Die macOS-Ausgabe muss auf einem Mac in eine `.app`-Bundle-Struktur verpackt und für die Verteilung mit Apple Developer ID signiert/notarisiert werden; Cross-Publishing erzeugt die Binärdateien, ersetzt aber weder Code-Signing noch Notarisierung.

## Bedienung

- Linke Maustaste: Fenster an freien Flächen verschieben; an Kanten und Ecken skalieren.
- Rechtsklick: Always on top, Gesamtfenster-Transparenz, Verstecken, Einstellungen, About oder Beenden.
- Tray-/Menüleisten-Icon: Linksklick schaltet das Fenster sichtbar/unsichtbar; das Rechtsklickmenü bietet dieselben Aktionen.
- Die Uhr nutzt automatisch das 12-/24-Stundenformat der aktuellen System-Culture.
- Der Ring besitzt exakt 60 Positionen, beginnt oben, füllt im Uhrzeigersinn und wird bei Sekunde 0 geleert. Fünf-Sekunden-LEDs sind auf demselben Kreis größer.

## Einstellungen

Gespeichert werden Fensterposition/-größe, Always-on-top, Transparenzschalter und -wert, Uhr-, Hintergrund- und Ringfarbe sowie Autostart. Fehlende, unvollständige oder beschädigte JSON-Dateien werden sicher auf Standardwerte zurückgeführt.

- Windows: `%APPDATA%\StudioClock\settings.json`
- macOS: `~/Library/Application Support/StudioClock/settings.json` (über `Environment.SpecialFolder.ApplicationData`)

Autostart verwendet unter Windows `HKCU\Software\Microsoft\Windows\CurrentVersion\Run` ohne Administratorrechte. Unter macOS wird der Benutzer-LaunchAgent `~/Library/LaunchAgents/de.studioclock.app.plist` angelegt. Nach Verschieben einer App sollte Autostart einmal aus- und wieder eingeschaltet werden, damit der Programmpfad aktualisiert wird.

## Architektur und Abhängigkeiten

- `Controls`: plattformfreies Dot-Matrix-/Ring-Rendering und testbare Geometrie
- `Models`, `Helpers`, `Services`: Settings, Koordination und lokale Named-Mutex-/Named-Pipe-Single-Instance-Logik
- `Platform/Windows`, `Platform/MacOS`: gekapselter Autostart
- `Views`: Hauptfenster und Dialoge
- `StudioClock.Tests`: Logik-, Persistenz- und Fensterplatzierungstests

Externe Pakete sind auf Avalonia Desktop, Fluent Theme, Inter-Fallbackfont und Avalonias ColorPicker begrenzt; xUnit und Microsoft.NET.Test.Sdk werden nur für Tests verwendet.

## Versionierung

Versionen folgen `YYYYMM.CNT` ohne führende Null im Monatszähler. Die zentrale Version in `StudioClock.csproj` ist `202608.1` und wird im About-Dialog aus den Assemblyinformationen gelesen.

## Plattformspezifische Hinweise

Die Windows-Funktionen werden lokal gebaut und getestet. macOS-Status-Item, LaunchAgent, Intel-/Apple-Silicon-Publish und `.app`-Packaging müssen abschließend auf echter macOS-Hardware geprüft werden. Verhalten von Tray-Linksklicks kann vom Desktop-Environment abhängen. Monitorpositionen werden beim Start gegen Avalonias aktuelle Screen-Arbeitsbereiche geprüft; bei entfernten Displays wird auf den primären Bildschirm zentriert.
