# 3D Zwilling Stiglmaierplatz – MR Prototyp

## 1. Konzept & Ziel

Dieses Projekt ist ein **Mixed Reality (MR) Digital Twin** des Stiglmaierplatzes in München (inklusive U-Bahn-Station). Ziel ist es, Einsatzkräften zu ermöglichen, komplexe Infrastrukturen intuitiv in 3D zu erfassen, Einsatzszenarien zu planen und sich räumlich zu orientieren.

Der Fokus liegt auf der **Verschmelzung von physischem Modell und digitaler Ebene**. Das digitale Modell wird passgenau über ein physisches 3D-Modell (Tabletop) gelegt.

## 2. Kern-Funktionen (Prototyp)

### A. Digitales Modell & Ebenen 

Der Benutzer kann interaktiv durch die Stockwerke "blättern", um unterirdische Strukturen sichtbar zu machen.

- **Steuerung:** Rechter Joystick (Hoch/Runter).
- **Funktion:** Schält Schicht für Schicht (Straße -> U1 -> U2) ab, um Einblick in die Tiefe zu gewähren.
- **Script:** `FloorManager.cs`

### B. Karten-Visualisierungen 

Zusätzlich zur Struktur können verschiedene thematische Karteninformationen auf die Oberfläche projiziert werden.

- **Steuerung:** **X-Knopf**.
- **Funktion:** Schaltet durch verschiedene Ansichts-Modi:
    - Satellitenbild (Realität)
    - Verkehrsauslastung (Heatmap/Daten)
    - ÖPNV/Straßenbahn-Linien
- **Nutzen:** Liefert kontextbezogene Zusatzinformationen direkt auf dem Modell.

### C. Einsatz-Szenario: Feuerwehr-Anfahrt

Eine prototypische Sequenz demonstriert den Informationsfluss und die Anfahrt eines Einsatzfahrzeugs.

- **Trigger:** **B-Knopf** (Rechter Controller).
- **Ablauf (Sequence):**
    1. **Inspect:** Ein "Hoverstate" (Pop-up) erscheint über dem physischen Smart-Objekt, welches das HLF repräsentiert. Außerdem erscheint ein blauer Ring unter dem HLF Smart-Objekt, welches die Wasserreichweite darstellt.
    2. **Status-Update:** Das Tooltip im Feuerwehrauto färbt sich **Grün** ("Info erhalten").
    3. **Anfahrt:** Das Feuerwehrauto (`FireTruck1`) fährt autonom zum Zielpunkt.
    4. **Reset:** Ein erneuter Klick auf **B** setzt die gesamte Szene sofort zurück ("Instant Reset").
- **Script:** `FiretruckNavigator.cs`

### D. Manuelles Alignment (Pass-Through)

Das System ist darauf ausgelegt, das digitale Modell manuell exakt über dem physischen Modell auf dem Tisch zu platzieren.

- **Steuerung:** **Grip-Taste** (Seitliche Taste).
- **Funktion:** Das gesamte Modell (`Stiglmaierplatz`) ist greifbar ("Grabbable"). Du kannst es nehmen, verschieben und drehen, um es perfekt auszurichten.
- **Rotation Lock:** Damit das Modell nicht versehentlich kippt, ist die Rotation auf die Y-Achse beschränkt (bleibt immer parallel zum Boden).
- **Script:** `StayUpright.cs` (auf dem Handle/Modell).

## 3. Bedienungsanleitung (Controls)

| **Taste (Quest 3)** | **Funktion**                                       |
| ------------------- | -------------------------------------------------- |
| **Grip (Halten)**   | Modell greifen & verschieben (Alignment).          |
| **Joystick (R)**    | Ebenen wechseln (Floor Peeling: Straße / U1 / U2). |
| **X-Knopf**         | Karten-Modus wechseln (Satellit / Verkehr / ÖPNV). |
| **B-Knopf**         | Szenario starten / Reset (Feuerwehr-Sequence).     |

## 4. Projektkontext & Credits

Dieses Projekt entstand im Rahmen des Moduls **Rettung-Gestalten** an der **Designfakultät der Hochschule München**.

**Betreuung:**

- Prof. Matthias Edler-Golla
- Prof. Florian Petri

**In Kollaboration mit:**

- Feuerwehr München

**Entwicklung & Umsetzung:**

- John Beno Lakshman
- Oliver Schlieper

