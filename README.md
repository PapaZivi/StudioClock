# StudioClock

StudioClock ist eine minimalistische, randlose Desktop-Uhr für Windows und macOS. Sie kann frei auf dem Bildschirm platziert werden und bleibt beispielsweise während Online-Meetings unauffällig sichtbar, ohne den Blick unnötig vom Geschehen abzulenken.

## Funktionen

- Randloses, frei verschiebbares und skalierbares Uhrenfenster
- Ruhige LED-Matrix für Stunden und Minuten
- LED-Sekundenring mit hervorgehobenen Fünf-Sekunden-Markierungen
- Always on top und einstellbare Transparenz
- Wählbare Uhr-, Hintergrund- und Ringfarben
- Anzeigen und Verstecken über Tray- beziehungsweise Menüleisten-Icon
- Optionaler Autostart und persistente Einstellungen
- Windows x64/x86 sowie macOS auf Apple Silicon und Intel

## Download

Fertige Versionen stehen unter [GitHub Releases](https://github.com/PapaZivi/StudioClock/releases) bereit:

- `StudioClock64.exe` – aktuelles 64-Bit-Windows
- `StudioClock32.exe` – 32-Bit-Windows
- `StudioClock-AppleSilicon.zip` – aktuelle Macs mit Apple Silicon
- `StudioClock-Intel.zip` – ältere Intel-Macs

Unter Windows kann die EXE direkt gestartet werden. Unter macOS das ZIP entpacken und die enthaltene App verwenden. Hinweise des Betriebssystems zu nicht signierten beziehungsweise nicht notarisierten Builds beachten.

## Bedienung

- Linksklick und Ziehen auf der Uhr: Fenster verschieben
- Rand oder Ecke ziehen: Größe ändern
- Rechtsklick auf die Uhr: Kontextmenü öffnen
- Linksklick auf das Tray-/Menüleisten-Icon: anzeigen oder verstecken
- Rechtsklick auf das Tray-/Menüleisten-Icon: Menü öffnen

## Einstellungen

StudioClock speichert Fensterposition und -größe, Always on top, Transparenz, Farben und Autostart lokal.

- Windows: `%APPDATA%\StudioClock\settings.json`
- macOS: `~/Library/Application Support/StudioClock/settings.json`

## Lizenz

StudioClock is open-source software licensed under the GNU Affero General Public License Version 3 only (`AGPL-3.0-only`).

Commercial use is permitted under the AGPL as long as its terms are followed.

For use cases where the AGPL requirements are not suitable, a separate commercial/proprietary license may be available.

Commercial licensing inquiries: `info@papazivi.de`

See [LICENSE](LICENSE) and [COMMERCIAL.md](COMMERCIAL.md). Entwickler und Selbstbauer finden technische Hinweise in [DEVELOPMENT.md](DEVELOPMENT.md).

Copyright © 2026 PapaZivi
