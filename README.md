# eGK-KVK
API zum Zugriff auf Daten der elektronischen Gesundheitskarte (eGK) und Krankenversichertenkarte (KVK)

Der Zugriff auf Chipkarten mithilfe der CT-API ("CardTerminal Application Programming Interface") ist an sich nicht schwer. Der Hersteller des Kartenterminals liefert eine Programmbibliothek (DLL) mit, die standardisierte Schnittstellen implementiert. Das eigentlich Knifflige ist das Durchwühlen sämtlicher Spezifikationen, um die Rückgabewerte korrekt zu interpretieren. Die KVK- & eGK-API hilft hierbei und stellt eine einfache Möglichkeit dar, um Krankversichertenkarten und elektronische Gesundheitskarten auszulesen.

# Benutzung der Codes
Zunächst muss eine Verbindung zum Kartenterminal aufgebaut werden. Dies geschieht mithilfe der gerätespezifischen Implementierung der CT-API. Hierbei wird nicht mehr gemacht, als die drei Aufrufe der {{Init}}-, {{Data}}- und {{Close}}-Methode zu kapseln.
Im Projekt enthalten ist momentan die Implementierung für die Geräte _Cherry MTK+ ST-2052_ und _Reinert SCT CyberJack_. Für weitere Geräte muss nur die {{ICardTerminalApi}}-Schnittstelle implementiert werden.

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
   }
   else
   {
      cardTerminalClient.SelectEGK();
      EgkResult egkResult = cardTerminalClient.ReadEGK();
   }

   cardTerminalClient.EjectICC();
}
```

Anhand des Rückgabewertes des `RequestICC`-Kommandos kann erkannt werden, ob es sich bei der eingesteckten Chipkarte um eine Krankenversichertenkarte oder eine elektronische Gesundheitskarte handelt. Anschließend kann mithilfe der Methode {{ReadKVK}} bzw. {{ReadEGK}} der Datenbereich ausgelesen werden.

# Implementierungsdetails

Die ausgeführten Befehle, verwendete Spezifikationen und angewandten Algorithmen können im **[Dokumentations-Bereich](documentation)** nachgelesen werden.
