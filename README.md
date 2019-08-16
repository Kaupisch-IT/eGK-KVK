# eGK- & KVK-API (via CTAPI)

Der Zugriff auf Chipkarten - hier die **elektronische Gesundheitskarte (eGK)** oder die **Krankenversichertenkarte (KVK)** - mithilfe der CT-API ("CardTerminal Application Programming Interface") ist an sich nicht schwer. Der Hersteller des Kartenterminals liefert eine Programmbibliothek (DLL) mit, die standardisierte Schnittstellen implementiert. Das eigentlich Knifflige ist das Durchwühlen sämtlicher Spezifikationen, um die Rückgabewerte korrekt zu interpretieren. Die KVK- & eGK-API hilft hierbei und stellt eine einfache Möglichkeit dar, um Krankversichertenkarten und elektronische Gesundheitskarten auszulesen.

## Benutzung der Codes
Zunächst muss eine Verbindung zum Kartenterminal aufgebaut werden. Dies geschieht mithilfe der gerätespezifischen Implementierung der CT-API. Hierbei wird nicht mehr gemacht, als die drei Aufrufe der `Init`-, `Data`- und `Close`-Methode zu kapseln.
Im Projekt enthalten ist momentan die Implementierung für die Geräte _Cherry MTK+ ST-2052_ und _Reinert SCT CyberJack_. Für weitere Geräte muss nur die `ICardTerminalApi`-Schnittstelle implementiert werden.

Anschließend können die entsprechenden Befehle an das Kartenterminal gesendet und die Rückgabewerte ausgewerten werden:
```csharp
using (var cardTerminalClient = new CardTerminalClient(new CherrySt2052Api()))
{
   cardTerminalClient.ResetCT();
            
   string result = cardTerminalClient.RequestICC();
   if (result=="9000")
   {
      cardTerminalClient.SelectKVK();
      KvkResult kvkResult = cardTerminalClient.ReadKVK();
      // irgendetwas mit den ausgelesenen Daten der Krankenversichertenkarte machen
   }
   else
   {
      cardTerminalClient.SelectEGK();
      EgkResult egkResult = cardTerminalClient.ReadEGK();
      // irgendetwas mit den ausgelesenen Daten der elektronischen Gesundheitskarte machen
   }

   cardTerminalClient.EjectICC();
}
```

Anhand des Rückgabewertes des `RequestICC`-Kommandos kann erkannt werden, ob es sich bei der eingesteckten Chipkarte um eine Krankenversichertenkarte oder eine elektronische Gesundheitskarte handelt. Anschließend kann mithilfe der Methode `ReadKVK` bzw. `ReadEGK` der Datenbereich ausgelesen werden.

## Anbindung an die jeweiligen Kartenleseterminals

Für die korrekte Ausführung muss die jeweilige **CTAPI-Bibliothek des verwendeten Chipkarten-Herstellers** verwendet werden. Der hier angebotene Quellcode unterstützt erst einmal nur die Geräte *Cherry ST-2052* und *Reinert SCT CyberJack*. Damit man das `CardReader`-Projekt mit anderen Chipkarten-Lesegeräten benutzen kann, muss die jeweilige DLL-Datei des Kartenlesegerät-Herstellers, das die CT-API implementiert, eingebunden werden (Ordner [CT-API](CardReader/CT-API) im Projekt). Jeder Kartenlesegerät-Hersteller hat individuelle Treiber und DLL-Dateien für die angebotenen Kartenlese-Terminals.

Die einzige Anpassung am `CardReader`-Projekt ist dann die Einbindung der CTAPI-Implementierung des (neuen) Geräteherstellers. Im CT-API-Ordner einfach eine weitere Anbindung für das neue Kartenlesegerät hinzufügen. Dazu ist nichts weiter zu tun, als Namen (Name der Klasse, kann frei gewählt werden) und Pfade (Datei im `DllImport`-Attribut) anzupassen.

Die CT-API-Bibliothek des Herstellers muss drei Funktionen anbieten, über die man dann Infos aus den eingesteckten Chipkarten auslesen kann: `CT_init`, `CT_data` und `CT_close`. Gegebenenfalls muss der Programm- oder Treiber-Ordners des Herstellers nach DLL-Dateien durchsucht und z.B. mit dem [DLL Export Viewer](https://www.nirsoft.net/utils/dll_export_viewer.html) geguckt werden, welche DLL-Datei die drei Funktionen `CT_init`, `CT_close` und `CT_data` exportiert. Dass sollte dann die richtige DLL-Datei sein, die im `DllImport`-Attribut deiner eigenen `ICardTerminalApi`-Implementierung angegeben werden muss.

## Implementierungsdetails

Die ausgeführten Befehle, verwendete Spezifikationen und angewandten Algorithmen können im **[Dokumentations-Bereich](Documentation.md)** nachgelesen werden.
