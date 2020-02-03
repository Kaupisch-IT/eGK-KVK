# Allgemeines über die CT-API

Die CT-API besteht aus drei Methoden:
* **Init** zum Verbinden zu einem bestimmten Kartenleseterminal. Übergabeparameter sind die Terminal-ID und die Port-Nummer.
* **Close** zum Schließen der Verbindung zu einem Kartenleseterminal. Übergabeparameter ist die ID des Terminals, dessen Verbindung geschlossen werden soll. _**Achtung:** Manche Geräte gehen in einen Fehlerzustand über und geben dann nur undefinierte Werte zurück, wenn nicht immer ordentlich `Close` aufgerufen wird. Dann hilft oft nur das Trennen und Wiederverbinden der USB-Verbindung_
* **Data** ist das eigentlich "Herzstück" der CT-API. Hiermit werden [Kommandos](#kommandos) verschickt und Daten angefordert. Die Parameter sind:
	* Die ID des Kartenterminals, an das ein Kommando geschickt werden soll
	* Das Ziel (Kartenkommandos = 0, Terminalkommandos = 1) und die Quelle (PC = 2) des Kommandos 
	* Die Länge und den Bytecode des eigentlichen Kommandos
	* Die Länge und den Zeiger auf das erste Element für den Rückgabe-Bytecode.

Die `CtApi`-Schnittstelle dient dem Aufruf der nativen CT-API-Methoden der jeweiligen Gerätehersteller. Im Konstruktor muss der Pfad zur jeweiligen CT-API-DLL des Kartenterminal-Herstellers angegeben werden. 

# Kommandos
Für Kommandos gibt es zwei verschiedene Ziele: Das Terminal selbst oder die eingesteckte Karte.
* Terminal-Kommandos:
	* **ResetCT** versetzt das Gerät in einen definierten Grundzustand
	* **RequestICC** leitet folgende Auslesevorgänge ein (zudem erkennt man an der Rückgabe, ob es sich um eine KVK oder eGK handelt)
	* **EjectICC** beendet einen Auslesevorgang
* Karten-Kommandos:
	* **SelectKVK** wählt den Container mit den KVK-Daten für folgende Auslesevorgänge aus.
	* **ReadKVK** liest den KVK-Datensatz aus.
	* **SelectEGK** wählt den Container mit den eGK-Daten für folgende Auslesevorgänge aus.
	* **ReadVST** liest den eGK-Datensatz für den Versichertenstatus aus.
	* **ReadPD** liest den eGK-Datensatz für die Patientendaten aus.
	* **ReadVD** liest den eGK-Datensatz für die Versicherungsdaten aus.

Eine recht ausführliche Doku zu den eGK-Kommandos findet sich in [Integrationsanleitung medMobile](https://www.medline-online.com/fileadmin/medline_relaunch/Support/medMobile/medMobile_Integrationsanleitung.pdf) (via [medline - medMobile - Kartenlesegeräte für die Gesundheitskarte (eGK)](https://www.medline-online.com/service-support/medcompact.html)).

# Rückgabewerte
Die beiden letzten Bytes des von der `Data`-Methode zurückgegebenen Bytecodes enthält immer den Status-Code des ausgeführten Kommandos. Mithilfe der `GetStatusBytes`-Methode kann dieser leicht ermittelt werden. Der Status-Code wird dabei in hexadezimaler Notation angezeigt.
Die `ExpectStatusBytes`-Methode hilft dabei, die zurückgegebenen Statuscodes zu interpretieren. Das erste Byte kodiert dabei den Ausgang einer Operation:
* `61`, `90` - Process completed - Normal Processing
* `62`, `63` - Process completed - Warning
* `64`, `65` - Process aborted - Execution Error
* `67`..`6f` - Process aborted - Checking Error

## Krankenversichertenkarte (KVK)
Zum Auslesen der Krankenversichertendaten muss nur ein Bereich auf der Karte ausgelesen werden:
```csharp
public KvkResult ReadKVK()
{
   var bytes = this.ExecuteCommand(CommandSet.ReadKVK).ExpectStatusBytes("9000","6282");
   return new KvkResult(bytes);
}
```

Die der zurückgegebene Bytecode enthält sowohl Metadaten (Tag, Länge) als auch die eigentlichen Nutzdaten (Dokumentation siehe [Multifunktionale Kartenterminals - MKT](https://www.teletrust.de/publikationen/spezifikationen/mkt/)):

Falls das erste Byte 82, 92 oder A2 ist (jeweils Hexadezimaldarstellung), kommt als erstes ATR und Directory (siehe [Anhang 3 MKT-Anforderungen für Versichertenkarten, 1.6.2 Bit- und Hexadezimal-Struktur des ATR und Directory](https://www.teletrust.de/publikationen/spezifikationen/mkt/)). Die (also die ersten 30 Bytes) können dann ignoriert/übersprungen werden.

Dann kommen die eigentlichen Daten (siehe [MKT-Teil 5: SYN – ATR und Datenbereiche 4 Codierungstechnik 4.1](https://www.teletrust.de/publikationen/spezifikationen/mkt/)):

Als Codierungstechnik für Datenobjekte werden die "Basic Encoding Rules (BER)" der ISO-Codierungskonvention "Abstract Syntax Notation One (ASN.1)" verwendet. Ein Datenobjekt besteht danach aus:
* einem Datenobjekt-Kennzeichen ("Tag")
* einer Längenangabe ("Length") und
* einem Datenobjekt-Wert ("Value").

Die Auflistung der **Tags** mit Bedeutungen und Min-/Max-Längen findet man in [Anhang 3 MKT-Anforderungen für Versichertenkarten, 1.6.3 Datenstruktur des Application-file und Prüfvorgaben](https://www.teletrust.de/publikationen/spezifikationen/mkt/).

Die **Länge** ist in 1-3 Bytes codiert (siehe [MKT-Teil 5: SYN – ATR und Datenbereiche 4 Codierungstechnik 4.1](https://www.teletrust.de/publikationen/spezifikationen/mkt/)):
* _Length 0 .. 127_: one byte coding the length
* _Length 128 .. 255_: 1st byte: bit b8 = 1, b7-b1= 0000001 (number of subsequent length bytes); 2nd byte: Length
* _Length 256 .. 65535_: 1st byte: bit b8 = 1,b7-b1= 0000010; 2nd + 3rd byte: Length

Man muss also den Bytecode durchgehen, sich den Tag merken, dann die Länge ermitteln, und entsprechend der Länge dann die nächsten n Bytes auslesen und diese schließlich in eine Zeichenkette konvertieren (DIN_66003-codiert). Danach kommt dann das nächte Tag, Länge, usw. - bis man am Ende angekommen ist.

Im Code sieht das dann folgendermaßen aus (zu finden in der `KvkResult`-Klasse):
```csharp
private Dictionary<byte,string> Decode(byte[] bytes)
{
   Dictionary<byte,string> result = new Dictionary<byte,string>();
   Encoding encoding = Encoding.GetEncoding("DIN_66003");

   int start = (bytes[0]==0x82 || bytes[0]==0x92 || bytes[0]==0xa2) ? 30 : 0;
   for (int i=start;i<bytes.Length-1-2;i++) //letzte 2 Bytes (ReturnCode) auslassen
   {
      byte tag = bytes[i++];
      int length = this.ReadLength(bytes,ref i);
      string value = encoding.GetString(bytes,i+1,length);

      if (tag!=0x60)
      {
         result.Add(tag,value);
         i += length;
      }
   }

   return result;
}

private int ReadLength(byte[] bytes,ref int i)
{
   byte firstByte = bytes[i];
   if (firstByte==0x81) // 128..255
      return bytes[++i];
   else if (firstByte==0x82) // 256..65535
      return (bytes[++i]<<8) + bytes[++i];
   else // 0..127
      return firstByte;
}
```


## Elektronische Gesundheitskarte (eGK)
Die Daten der eGK bestehen aus den Patientendaten (PD) und den Versichertendaten (VD), die mit zwei separaten Kommandos ausgelesen werden können. 
```csharp
public EgkResult ReadEGK()
{
   byte[] pdData = this.ExecuteCommand(CommandSet.ReadPD).ExpectStatusBytes("9000","6282");
   byte[] vdData = this.ExecuteCommand(CommandSet.ReadVD).ExpectStatusBytes("9000","6282");
   return new EgkResult(pdData,vdData);
}
```

Der zurückgegebene Bytecode der Patientendaten (`pdData`; Name, Anschrift) enthält 2 Bytes Längenabgabe für den Inhalt, dann ein ZIP-Komprimiertes XML-Dokument (ISO-8859-15 codiert).
Der Bytecode mit den Versichertendaten (`vdData `) enthält jeweils 2 Bytes Offset für Start & Ende der allgemeinen Versicherungsdaten (Kostenträger & Versicherungsschutz), sowie je 2 Bytes Offset für Start & Ende der geschützten Versichertendaten (Zuzahlungsstatus & besondere Kennzeichen; diese lassen sich in den Musterkarten noch Auslesen, bei den "echten" eGKarten kommt man aber nicht mehr einfach ran).

Dokumentation dazu gibt es unter [Implementierungsleitfaden zur Einbindung der eGK in die Primärsysteme der Leistungserbringer](https://fachportal.gematik.de/spezifikationen/basis-rollout/gesundheitskarte/implementierungsleitfaden/). Unter 4.2.3 "Datei EF.PD" und 4.2.4 "Datei EF.VD" steht, wie jeweils das Byte-Array aufgebaut ist.

Die Daten selbst werden als XML-Daten gemäß vorgegebenem XML-Schema, gzip-komprimiert und nicht verschlüsselt innerhalb der Datei abgelegt. Der zu verwendende Zeichensatz für die fachlichen Inhalte ist ISO8859-15

Die Schemadateien kann man z.B. unter [Release 0.5.3 Basis-Rollout](https://fachportal.gematik.de/spezifikationen/basis-rollout/) herunterladen (ganz unten "Für Hersteller bietet die gematik zudem Schnittstellendefinitionen im XSD- und WSDL-Format an").
Aus den xsd-Dateien kann man mit dem [XML Schema Definition-Tool (Xsd.exe)](http://msdn.microsoft.com/de-de/library/x6c1kb0s.aspx) entsprechende Klassen generieren lassen.

Die auf der Gesundheitskarte enthaltenen, XML-kodierten Daten sind in verschiedenen Schemata-Versionen gespeichert (die Version kann aus dem Attribut `CDM_VERSION` ausgelesen werden; momentan werden die Versionen 5.1.0 und 5.2.0 unterstützt). Bei der Deserialisierung (Methode `EgkResult.Decompress`) werden die verschiedenen Namespaces ignoriert und die EGK-Daten in die gleichen Data-Transfer-Objekte übertragen (Klassen im Ordner `eGK-Data`). 

```csharp
private void DecodePD(byte[] bytes)
{
    int length = (bytes[0]<<8) + bytes[1];

    byte[] compressedData = new byte[length];
    Array.Copy(bytes,2,compressedData,0,compressedData.Length);
    
    this.PersoenlicheVersichertendaten = this.Decompress<PersoenlicheVersichertendaten>(compressedData);
}


private void DecodeVD(byte[] bytes)
{
    int offsetStartVD = (bytes[0]<<8) + bytes[1];
    int offsetEndVD = (bytes[2]<<8) + bytes[3];
    int offsetStartGVD = (bytes[4]<<8) + bytes[5];
    int offsetEndGVD = (bytes[6]<<8) + bytes[7];

    byte[] compressedDataVD = new byte[offsetEndVD-offsetStartVD];
    if (compressedDataVD.Length>0)
    {
        Array.Copy(bytes,offsetStartVD,compressedDataVD,0,compressedDataVD.Length);
        this.AllgemeineVersicherungsdaten = this.Decompress<AllgemeineVersicherungsdaten>(compressedDataVD);
    }

    byte[] compressedDataGVD = new byte[offsetEndGVD-offsetStartGVD];
    if (compressedDataGVD.Length>0)
    {
        Array.Copy(bytes,offsetStartGVD,compressedDataGVD,0,compressedDataGVD.Length);
        this.GeschuetzteVersichertendaten = this.Decompress<GeschuetzteVersichertendaten>(compressedDataGVD);
    }
}

private T Decompress<T>(byte[] compressedData)
{
    using (MemoryStream memoryStream = new MemoryStream(compressedData))
    using (GZipStream gzipStream = new GZipStream(memoryStream,CompressionMode.Decompress))
    using (StreamReader streamReader = new StreamReader(gzipStream,Encoding.GetEncoding("ISO-8859-15")))
    {
        string xmlContent = streamReader.ReadToEnd(); 
        
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        using (TextReader textReader = new StringReader(xmlContent))
        using (XmlTextReader xmlTextReader = new XmlTextReader(textReader) { Namespaces = false })
            return (T)xmlSerializer.Deserialize(xmlTextReader);
    }
}
```

Sonstiges:
* In der [Spezifikation für Multifunktionale Kartenterminals (MKT)](http://www.teletrust.de/publikationen/spezifikationen/mkt/) sind in [Teil 4 (CT-BCS - Anwendungsunabhängiger CardTerminal Basic Command Set)](http://www.teletrust.de/fileadmin/files/mkt1-0_t4.zip) die Kommandos und möglichen Rückgabe-Codes erklärt.
* eGK-Musterkarten können kostenfrei unter [gematik (Bestellung Musterkarten)](https://fachportal.gematik.de/spezifikationen/basis-rollout/gesundheitskarte/spezifikation-fuer-musterkarten-und-testkarten/) beantragt werden.

>{ **[www.kaupisch-itc.de/impressum](http://www.kaupisch-it.de/index.php?option=com_content&view=article&id=59&Itemid=18)** }>
