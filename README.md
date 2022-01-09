# eGK- & PKV-Card/KVK-API (via CTAPI)

Für den Zugriff die **elektronische Gesundheitskarte (eGK)** oder die **Card für Privatversicherte (PKV-Card)** (und bis 2014 auch die *Krankenversichertenkarte (KVK)*) gibt es die [CT-API](https://www.tuvit.de/de/aktuelles/white-paper-downloads/card-terminal-application-programing-interface-fuer-chipkartenanwendungen/) („CardTerminal Application Programming Interface“). Dafür liefert der Hersteller des Kartenterminals eine (Kartenterminal-spezifische) Programmbibliothek (DLL) mit, welche die standardisierte Schnittstellen implementiert.

Zwischen den beiden Kartentypen bestehen gravierende Unterschiede, sowohl was die Hardware betrifft (Speicher- versus Prozessorkarte) als auch bezüglich der Datenstruktur. Während die Daten auf der PKV-Card/KVK ASN.1-kodiert in einem einzigen linearen File abgelegt sind, besitzt die eGK ein hierarchisches Filesystem und verwendet für die Fachdaten, welche auf der eGK
gzip-komprimiert abgelegt sind, das XML-Format. Der Einlesevorgang für die eGK muss daher anderen Algorithmen folgen als der für die KVK.

Die _Kaupisch IT eGK- & PKV-Card/KVK-API_ stellt eine einfache Möglichkeit dar, um die elektronische Gesundheitskarte und die Card für Privatversicherte auszulesen.

## Auslesen von Versichertenstammdaten
Die `CardTerminalClient.ReadCard`-Methode stellt eine Verbindung mit einem Chipkartenterminal her und liest (falls eingesteckt) die Versichertenstammdaten einer eGK oder KVK/PKV-Card aus. Das zurückgegebene `CardResult`-Objekt stellt Informationen bereit, die sich sowohl auf einer eGK als auch auf einer PKV-Card befinden (Kostenträger und Basis-Informationen des Versicherten).

Wenn eine eGK eingelesen wurde, kann über das `EgkResult` auf die _Persönlichen Versichertendaten_ (PD), die _Allgemeinen Versicherungsdaten_ (VD) und die _Geschützten Versichertendaten_ (GVD) aus den Versichertenstammdaten zugegriffen werden; wenn eine PKV-Card eingelesen wurde, kann über das `PkvResult` auf die Krankenversichertendaten zugegriffen werden (eine KVK hat sich genau so verhalten, wie eine PKV-Card).

Für die korrekte Ausführung muss die jeweilige **CTAPI-Bibliothek des verwendeten Chipkarten-Herstellers** eingebunden werden. Zum Aufbau der Verbindung zu einem Kartenterminal muss daher der Pfad zur herstellerspezifischen DLL mit der CT-API-Implementierung angegeben werden. (Die Suchreihenfolge der DLL entspricht der normalen DLL-Suchreihenfolge - also erst Anwendungsverzeichnis, dann System-Verzeichnis, dann Windows-Verzeichnis usw.)
```csharp
CardResult result = CardTerminalClient.ReadCard("ctacs.dll");
if (result.Success)
{
   // ausgelesene Ergebnisse auswerten
}
```

Manche Kartenterminals (z.B. das _ingenico ORGA 6141_ oder _ingenico ORGA 930 M_) registrieren einen **COM-Port**. Dieser muss dann ggf. ebenfalls mit angegeben werden.
```csharp
CardResult result = CardTerminalClient.ReadCard("ctorg32.dll",portNumber: 4);
```

In der Regel unterstützen die Kartenlesegeräte auch die Angabe einer **Wartezeit** (Zeitspanne in Sekunden), die auf das Einstecken einer Chipkarte (`RequestICC`) gewartet wird bzw. die gewartet wird, bis eine eingesteckte Chipkarte entnommen wurde (`EjectICC`).
```csharp
CardResult result = CardTerminalClient.ReadCard("ctpcsc32kv.dll", requestCardWaitingPeriodInSeconds: 10, ejectCardWaitingPeriodInSeconds: 10);
```

## Anbindung an Kartenleseterminals

Folgende Geräte wurden bisher mit der eGK- & KVK-API getestet.<sup> [Weitere getestete Geräte können gern mit aufgenommen werden.](https://github.com/Kaupisch-IT/eGK-KVK/issues/2) </sup>  
(:green_heart: = Daten konnten ausgelesen werden, :broken_heart: = Daten konnten _nicht_ ausgelesen werden)

| Gerätename | CT-API-DLL | eGK | PKV-Card/KVK |
| ------------- |-------------| :-----:| :-----:|
| ingenico ORGA 6141 (USB-Modus) | `ctorg32.dll`<sup>*)</sup> | :green_heart: | :green_heart: |
| ingenico ORGA 930 M (USB-Modus) | `ctorg32.dll`<sup>*)</sup> | :green_heart: | :green_heart: |
| Cherry eHealth Terminal ST-2052 | `ctpcsc32kv.dll` | :green_heart: | :green_heart: |
| Cherry eHealth Terminal ST-2100 | `ctpcsc32kv.dll` | :green_heart: | :green_heart: |
| Cherry SmartTerminal ST-1144  | `ctcym.dll`/`CTChyCTApiSp.dll` | :green_heart: | :green_heart: |
| Cherry eHealth Keyboard G87-1504 | `ctcym.dll`<sup>[Details](https://github.com/Kaupisch-IT/eGK-KVK/issues/2#issuecomment-1008163064)</sup> | :green_heart: | :green_heart: |
| REINER SCT cyberJack e-com plus | `ctrsct32.dll` | :green_heart: | :green_heart: |
| REINER SCT cyberJack RFID | `ctrsct32.dll` | :green_heart: | :broken_heart: |
| SCR 3310 SCM USB SmartCard Leser | `CTPCSC31kv.dll`<sup>[Details](https://github.com/Kaupisch-IT/eGK-KVK/issues/2#issuecomment-1008163064)</sup> | :green_heart: | :green_heart: |
| ACS ACR39U PocketMate II | `ctacs.dll` | :green_heart: | :broken_heart: |
| HID Omnikey 3021 USB | `ctcym.dll`/`CTChyCTApiSp.dll`<sup>**)</sup> | :green_heart: | :broken_heart: |
| HID Omnikey 3121 USB | `ctcym.dll`/`CTChyCTApiSp.dll`<sup>**)</sup> | :green_heart: | :broken_heart: |
| CSL USB SmartCard Reader | `ctcym.dll`/`CTChyCTApiSp.dll`<sup>**)</sup> | :green_heart: | :broken_heart: |

<sup>*) Die Angabe des Parameters `portNumber` (Nummer des COM-Ports) ist hier notwendig.</sup>
   
**CT-API-DLL**: Gegebenenfalls muss der Programm- oder Treiber-Ordners des Herstellers nach DLL-Dateien durchsucht und z.B. mit dem [DLL Export Viewer](https://www.nirsoft.net/utils/dll_export_viewer.html) geguckt werden, welche DLL-Datei die drei Funktionen `CT_init`, `CT_close` und `CT_data` exportiert. Das sollte dann die richtige DLL-Datei sein, die als Parameter an die `CardTerminalClient`-Klasse übergeben werden muss.

<sup>**) Einige Kartelesegeräte stellen keine eigene CT-API-Anbindung (mehr) zur Verfügung, allerdings scheint die CT-API-Implementierung aus den *[Cherry](https://cherry.de/download/de/download.php) CardReaderTools* auch für andere Lesegeräte verwendbar zu sein (zumindest zum Auslesen der eGK).</sup>

Test-/Musterkarten:
* **eGK-Testkarten** können unter [gematik Fachportal Service/Testkarten](https://fachportal.gematik.de/service/testkarten/) beantragt werden.
* **PKV-Card-Musterkarten** (verhalten sich wie die alte KVK) können beim [Verband der Privaten Krankenversicherung](https://www.pkv.de/) beantragt werden.


## Auslesen von Versichertenstammdaten (detaillierter)

Die `ReadCard`-Methode ist folgendermaßen implementiert:
```csharp
public static CardResult ReadCard(string path,ushort portNumber = 1,ushort terminalID = 1,byte requestCardWaitingPeriodInSeconds = 0,byte ejectCardWaitingPeriodInSeconds = 0)
{
   using (CardTerminalClient cardTerminalClient = new CardTerminalClient(path,portNumber,terminalID)) // eine neue Host/CT-Verbindung mithilfe der herstellerspezifischen CT-API-Bibliothek initiieren
   {
      cardTerminalClient.ResetCT(); // das Gerät in einen definierten Grundzustand versetzen
      cardTerminalClient.RequestICC(requestCardWaitingPeriodInSeconds); // zum Einstecken einer Chipkarte auffordern (ggf. mit Wartezeit) und nach dem Einstecken einer Karte einen Reset durchführen

      CardResult result = new CardResult();

      // Daten einer elektronischen Versichertenkarte (eGK) auslesen
      try
      {
         if (!cardTerminalClient.SelectEGK().StatusIsError()) // Container mit den eGK-Daten für folgende Auslesevorgänge auswählen
            result.EgkResult = cardTerminalClient.ReadEGK(); // ggf. eGK-Datensätze für die Patientendaten und die Versicherungsdaten auslesen
      }
      catch (CtException ex) when (ex.ErrorCode==-128) { } // ERR_HTSI

      // Daten einer Card für Privatversicherte (PVK-Card) bzw. Krankenversichertenkarte (KVK) auslesen
      try
      {
         if (!cardTerminalClient.SelectKVK().StatusIsError()) // Container mit den Krankenversichertendaten für folgende Auslesevorgänge auswählen
            result.PkvResult = cardTerminalClient.ReadKVK(); // ggf. Krankenversichertendatensatz auslesen
      }
      catch (CtException ex) when (ex.ErrorCode==-128) { } // ERR_HTSI

      cardTerminalClient.EjectICC(ejectCardWaitingPeriodInSeconds); // Auslesevorgang beenden und Chipkarte auswerfen (ggf. mit Wartezeit)

      return result;
   }
}
```
Hinweis: Bei nicht eingesteckter Karte signalisiert der `RequestICC`-Befehl in der Regel keinen Fehler, sondern nur eine Warnung *(6200 - Warning: no card presented within specified time)*; ebenso wird eine Warnung ausgegeben, wenn bereits eine Karte steckte *(6201 - Warning: ICC already present and activated)*.

Anhand der Rückgabewerte der `SelectEGK`- bzw. `SelectKVK`-Methoden kann erkannt werden, ob eGK- bzw. PKV-Card/KVK-Daten eingelesen werden kann (also ob es sich bei der eingesteckten Karte um eine elektronische Gesundheitskarte oder um eine Card für Privatversicherte/Krankenversichertenkarte handelt). Einige Geräte quittieren eine Nichtunterstützung der Auslesebefehle jedoch nicht durch entsprechende Rückgabewerte, sondern verursachen eine HTSI-Exception.

### Beispielhafter Ablauf von Auslesevorgängen mit Rückgabecodes

Aufruf von `ReadCard` mit __bereits eingesteckter eGK__ (keine Wartezeiten angegeben):
```
ResetCT     9000  (Reset successful)
RequestICC  6201  (Warning: ICC already present and activated)
SelectEGK   9000  (Command successful - erfolgreiche Selektion eines Files)
ReadEGK     9000  (Command successful - Datei gelesen)
ReadEGK     9000  (Command successful - Datei gelesen)
SelectKVK   6a86  (Error: Command parameters not supported)
EjectICC    6200  (Warning: Card not removed within specified time)
```

Aufruf von `ReadCard` mit Wartezeit, __eGK__ innherhalb der Wartezeit eingesteckt und entfernt:
```
ResetCT     9000  (Reset successful)
RequestICC  9001  (Asynchronous ICC presented, reset successful)
SelectEGK   9000  (Command successful - erfolgreiche Selektion eines Files)
ReadEGK     9000  (Command successful - Datei gelesen)
ReadEGK     9000  (Command successful - Datei gelesen)
SelectKVK   6a86  (Error: Command parameters not supported)
EjectICC    9001  (Command successful, card removed)
```

Aufruf von `ReadCard` mit Wartezeit, __PKV-Card__ innherhalb der Wartezeit eingesteckt und entfernt:
```
ResetCT     9000  (Reset successful)
RequestICC  9000  (Synchronous ICC presented, reset successful)
SelectEGK   6a00  (Error: Falsche Parameter P1, P2)
SelectKVK   9000  (Command successful - erfolgreiche Selektion eines Files)
ReadKVK     9000  (Command successful - Datei gelesen)
EjectICC    9001  (Command successful, card removed)
```

Aufruf von `ReadCard`, __keine Karte__ eingesteckt:
```
ResetCT     9000     (Reset successful)
RequestICC  6200     (Warning: no card presented within specified time)
Ausnahme ausgelöst: "KaupischIT.CardReader.CtException" in KaupischIT.CardReader.dll
Nicht näher spezifizierter Fehler, den das HTSI nicht interpretieren kann und die zu einem Abbruch der Funktion geführt haben; Neuinitialisierung des CT erforderlich.
Ausnahme ausgelöst: "KaupischIT.CardReader.CtException" in KaupischIT.CardReader.dll
Nicht näher spezifizierter Fehler, den das HTSI nicht interpretieren kann und die zu einem Abbruch der Funktion geführt haben; Neuinitialisierung des CT erforderlich.
EjectICC    9001     (Command successful, card removed)
```
Rückgabecodes unterscheiden sich ggf. je nach Gerätehersteller.

## Implementierungsdetails

Die ausgeführten Befehle, verwendete Spezifikationen und angewandten Algorithmen können im **[Dokumentations-Bereich](Documentation.md)** nachgelesen werden.
