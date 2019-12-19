# eGK- & KVK-API (via CTAPI)

Der Zugriff auf Chipkarten - hier die **elektronische Gesundheitskarte (eGK)** oder die **Krankenversichertenkarte (KVK)** - mithilfe der [CT-API](https://www.tuvit.de/de/aktuelles/white-paper-downloads/card-terminal-application-programing-interface-fuer-chipkartenanwendungen/) ("CardTerminal Application Programming Interface") ist an sich nicht schwer. Der Hersteller des Kartenterminals liefert eine Programmbibliothek (DLL) mit, die standardisierte Schnittstellen implementiert. Das eigentlich Knifflige ist das Durchwühlen sämtlicher Spezifikationen, um die Rückgabewerte korrekt zu interpretieren. Die KVK- & eGK-API hilft hierbei und stellt eine einfache Möglichkeit dar, um Krankversichertenkarten und elektronische Gesundheitskarten auszulesen.

## Benutzung der Codes
Zunächst muss eine Verbindung zum Kartenterminal aufgebaut werden. Dies geschieht mithilfe der gerätespezifischen Implementierung der CT-API. Hierbei wird nicht mehr gemacht, als die drei Aufrufe der `Init`-, `Data`- und `Close`-Methode zu kapseln.

Anschließend können die entsprechenden Befehle an das Kartenterminal gesendet und die Rückgabewerte ausgewerten werden:
```csharp
using (CardTerminalClient cardTerminalClient = new CardTerminalClient("ctacs.dll"))
{
    cardTerminalClient.ResetCT();

    string result = cardTerminalClient.RequestICC();
    if (result=="9000")
    {
        cardTerminalClient.SelectKVK();
        KvkResult kvkResult = cardTerminalClient.ReadKVK();

        string json = JsonConvert.SerializeObject(kvkResult);
        MessageBox.Show(json);
    }
    else
    {
        cardTerminalClient.SelectEGK();
        EgkResult egkResult = cardTerminalClient.ReadEGK();

        string json = JsonConvert.SerializeObject(egkResult);
        MessageBox.Show(json);
    }

    cardTerminalClient.EjectICC();
}
```

Anhand des Rückgabewertes des `RequestICC`-Kommandos kann erkannt werden, ob es sich bei der eingesteckten Chipkarte um eine Krankenversichertenkarte oder eine elektronische Gesundheitskarte handelt. Anschließend kann mithilfe der Methode `ReadKVK` bzw. `ReadEGK` der Datenbereich ausgelesen werden.

## Anbindung an die jeweiligen Kartenleseterminals

Für die korrekte Ausführung muss die jeweilige **CTAPI-Bibliothek des verwendeten Chipkarten-Herstellers** eingebunden werden. 
Dazu muss der Pfad zur DLL-Datei, die die CT-API des zu verwendenen Kartenlese-Terminals implementiert, als Parameter angegeben werden.

Beispielsweise ist das:

 - Cherry ST2052: `ctpcsc32kv.dll`
 - Orga 6141: `CTORG32.dll`
 - Reinert SCT CyberJack: `ctrsct32.dll`
 - ACS PocketMate: `ctacs.dll`
 - Cherry1504: `ctcym.dll`

Gegebenenfalls muss der Programm- oder Treiber-Ordners des Herstellers nach DLL-Dateien durchsucht und z.B. mit dem [DLL Export Viewer](https://www.nirsoft.net/utils/dll_export_viewer.html) geguckt werden, welche DLL-Datei die drei Funktionen `CT_init`, `CT_close` und `CT_data` exportiert. Dass sollte dann die richtige DLL-Datei sein, die als Parameter an die `CardTerminalClient`-Klasse übergeben werden muss. 

## Implementierungsdetails

Die ausgeführten Befehle, verwendete Spezifikationen und angewandten Algorithmen können im **[Dokumentations-Bereich](Documentation.md)** nachgelesen werden.
