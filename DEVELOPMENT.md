# StudioClock development

Diese Datei richtet sich an Contributor, Fork-Autoren und Personen, die StudioClock selbst bauen möchten.

## Voraussetzungen

- .NET 10 SDK
- Windows 10/11 für lokale Windows-Tests
- macOS für abschließende Tests, App-Bundling, Signierung und Notarisierung

## Repository-Struktur

- `StudioClock/Controls`: Uhrdarstellung und testbare Geometrie
- `StudioClock/Models`: persistente Datenmodelle
- `StudioClock/Helpers`: gemeinsame Hilfs- und Validierungslogik
- `StudioClock/Services`: Settings, Anwendungskoordination und Single Instance
- `StudioClock/Platform`: plattformspezifischer Autostart und Farbauswahl
- `StudioClock/Views`: Hauptfenster und Dialoge
- `StudioClock.Tests`: Unit Tests

## Build und Tests

```bash
dotnet restore StudioClock.slnx
dotnet build StudioClock.slnx -c Release
dotnet test StudioClock.slnx -c Release
```

Temporäre Ausgaben liegen in ignorierten `bin`, `obj`, `publish*` und `TestResults`-Verzeichnissen.

## Publish

Die folgenden Beispiele entsprechen den wesentlichen Workflow-Einstellungen:

```bash
dotnet publish StudioClock/StudioClock.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PublishTrimmed=true
dotnet publish StudioClock/StudioClock.csproj -c Release -r win-x86 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PublishTrimmed=true
dotnet publish StudioClock/StudioClock.csproj -c Release -r osx-arm64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false
dotnet publish StudioClock/StudioClock.csproj -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false
```

### Windows

Windows-Publishes sind self-contained, Single File und getrimmt. `ApplicationIcon` bindet `StudioClock/Assets/StudioClock.ico` als Win32-Icon ein. Unterstützt werden x64 und x86.

### macOS

Der Workflow erzeugt für Apple Silicon und Intel jeweils ein selbstenthaltendes Single-File-Publish, legt daraus `StudioClock.app` an und verpackt das Bundle als ZIP. Signierung und Notarisierung sind derzeit nicht Bestandteil des Workflows und müssen für entsprechend verteilte Builds separat erfolgen.

## Settings

`SettingsService` serialisiert `AppSettings` mit System.Text.Json Source Generation. Fehlende Werte erhalten kompatible Defaults; Speichern erfolgt über eine temporäre Datei mit anschließendem Austausch.

- Windows: `%APPDATA%\StudioClock\settings.json`
- macOS: `~/Library/Application Support/StudioClock/settings.json`

Gespeichert werden Fenstergeometrie, Always on top, Transparenz, Farben und Autostart. Eine bestehende Datei darf bei Schemaerweiterungen nicht allein wegen fehlender neuer Properties verworfen werden.

## Tests

Das xUnit-Projekt `StudioClock.Tests` prüft insbesondere Renderinglogik, Settings-Kompatibilität, Fensterplatzierung und manuellen Resize. Vor einer Änderung mindestens `dotnet test StudioClock.slnx -c Release` ausführen; Publish-relevante Änderungen zusätzlich für die betroffenen RIDs testen.

## GitHub Actions

`.github/workflows/build.yaml` läuft manuell (`workflow_dispatch`) und beim Veröffentlichen eines GitHub Release. Er testet unter Windows und erzeugt:

- `StudioClock64.exe`
- `StudioClock32.exe`
- `StudioClock-AppleSilicon.zip`
- `StudioClock-Intel.zip`

Bei einem Release-Ereignis werden diese Artefakte an das veröffentlichte GitHub Release angehängt.

## Versionierung

- Release: `YYYY.MM.R`
- Patch: `YYYY.MM.R.P`
- Patchlevel 0 wird nicht geschrieben.

Beispiele: `2026.08.3`, `2026.08.3.1`, `2026.08.4`.

## Contributions

Beiträge müssen die Rechte Dritter respektieren und dürfen nur mit den erforderlichen Rechten eingereicht werden. Vor der Übernahme in das Hauptprojekt ist die Zustimmung zu [CLA.md](CLA.md) erforderlich. Lizenzfragen können an `info@papazivi.de` gerichtet werden.
