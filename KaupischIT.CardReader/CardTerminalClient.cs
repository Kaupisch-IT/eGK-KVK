using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KaupischIT.CardReader
{
	/// <summary>
	/// Stellt Funktionen zum Zugriff auf die Dateien einer elektronischen Gesundheitskarte (eGK) oder Krankenversichertenkarte (KVK) bzw. Card für Privatversicherte (PKV-Card) bereit
	/// </summary>
	public sealed class CardTerminalClient : IDisposable
	{
		private readonly CtApi cardTerminalApi; // Wrapper für die herstellerspezifische CT-API
		private readonly ushort terminalID;     // die ID des Kartenterminals


		/// <summary>
		/// Initialisiert eine neue Instanz der CardTerminalClient-Klasse und initiiert eine neue Host/CT-Verbindung
		/// </summary>
		/// <param name="path">der Pfad zur herstellerspezifischen DLL mit der CT-API-Implementierung</param>
		/// <param name="portNumber">Portnummer der physikalischen Schnittstelle</param>
		/// <param name="terminalID">die logische Kartenterminal-Nummer</param>
		public CardTerminalClient(string path,ushort portNumber = 1,ushort terminalID = 1)
		{
			this.cardTerminalApi = new CtApi(path);
			this.terminalID = terminalID;

			// Initiieren der Host/CT-Verbindung
			this.cardTerminalApi.Init(this.terminalID,portNumber);
		}


		/// <summary>
		/// Stellt eine Verbindung mit einem Chipkartenterminal her und liest (falls eingesteckt) die Versichertenstammdaten einer eGK oder KVK/PKV-Card aus
		/// </summary>
		/// <param name="path">der Pfad zur herstellerspezifischen DLL mit der CT-API-Implementierung</param>
		/// <param name="portNumber">Portnummer der physikalischen Schnittstelle des Kartenterminals</param>
		/// <param name="terminalID">die logische Kartenterminal-Nummer</param>
		/// <param name="requestCardWaitingPeriodInSeconds">die Zeit (in Sekunden), die auf das Einstecken einer Chipkarte gewartet wird</param>
		/// <param name="ejectCardWaitingPeriodInSeconds">die Zeit (in Sekunden), die gewartet wird, bis eine eingesteckte Chipkarte entnommen wurde</param>
		/// <returns></returns>
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

				// Daten einer Krankenversichertenkarte (KVK) bzw. Card für Privatversicherte (PVK-Card) auslesen
				try
				{
					if (!cardTerminalClient.SelectKVK().StatusIsError()) // Container mit den KVK-Daten für folgende Auslesevorgänge auswählen
						result.KvKResult = cardTerminalClient.ReadKVK(); // ggf. KVK-Datensatz auslesen
				}
				catch (CtException ex) when (ex.ErrorCode==-128) { } // ERR_HTSI

				cardTerminalClient.EjectICC(ejectCardWaitingPeriodInSeconds); // Auslesevorgang beenden und Chipkarte auswerfen (ggf. mit Wartezeit)

				return result;
			}
		}


		/// <summary>
		/// Sendet ein Kommando an ein Kartenterminal/eine Chipkarte und gibt die Antwort zurück
		/// </summary>
		/// <param name="dad">Destination Address - Empfänger des Kommandos</param>
		/// <param name="sad">Source Address - Sender des Kommandos</param>
		/// <param name="command">Chipkarten- bzw. Kartenterminal-Kommando</param>
		/// <returns>eine Bytefolge mit der Antwort auf das Kommando</returns>
		[DebuggerStepThrough]
		private byte[] ExecuteCommand(byte dad,byte sad,byte[] command)
		{
			ushort responseLength = UInt16.MaxValue;
			byte[] response = new byte[responseLength];

			// Senden eines Kommandos an ein CardTerminal bzw. an eine Chipkarte und Rückgabe der Antwort
			this.cardTerminalApi.Data(this.terminalID,ref dad,ref sad,(ushort)command.Length,ref command[0],ref responseLength,ref response[0]);
			byte[] result = new byte[responseLength];
			Array.Copy(response,result,responseLength);

			return result;
		}


		/// <summary>
		/// Beendet die Host/CT-Verbindung und gibt verwendete Ressourcen frei
		/// </summary>
		public void Dispose()
		{
			// Beenden der Host/CT-Verbindung
			this.cardTerminalApi.Close(this.terminalID);
			this.cardTerminalApi.Dispose();
		}


		/// <summary> 
		/// Versetzt das Gerät in einen definierten Grundzustand. 
		/// Bei Kartenterminals mit mehr als einer Schnittstelle werden die gesperrten Ports wieder freigegeben.
		/// </summary>
		public byte[] ResetCT()
		{
			return this.ExecuteCommand(
				sad: 2, // source = Host
				dad: 1, // destination = Card Terminal
				command: new byte[]
				{ 
					// RESET CT (Kartenleser zurücksetzen)
					0x20, // CLA '20'
					0x11, // INS '11'
					0x00, // (P1) Device ('00' = terminal, '01' = ICC1, '02' = ICC2)
					0x00, // (P2) Resp. Type ('00' = no responses, '01' = entire ATR, '02' = only historical characters as response)
					// (Lc) Empty
					// (Data field) Empty
					// (Le) Empty or '00'
				})
				.CheckStatusBytes(new Dictionary<string,string>
				{
					{ "9000", "Reset successful" },
					{ "9001", "Asynchronous ICC, reset successful" },
					{ "6400", "Reset not successful" },
				});
		}


		/// <summary> 
		/// Fordert zum Einstecken einer Chipkarte auf - mit der Möglichkeit, eine Wartezeit anzugeben - und führt nach dem Einstecken einer Karte einen Reset durch.
		/// Kartenterminals, die mit einem Display ausgestattet sind, bieten die Möglichkeit, eine Eingabeaufforderung anzuzeigen.
		/// </summary>
		/// <param name="waitingPeriodInSeconds">Zeitraum (in Sekunden), der gewartet wird, bis eine Chipkarte eingesteckt wurde.</param>
		public byte[] RequestICC(byte waitingPeriodInSeconds = 0)
		{
			return this.ExecuteCommand(
				sad: 2, // source = Host
				dad: 1, // destination = Card Terminal
				command: new byte[]
				{ 
					// REQUEST ICC1 (Kartenanforderung)
					0x20, // CLA '20'
					0x12, // INS '12'
					0x01, // (P1) Device '01' = ICC1, '02' = ICC2 (only B1 Professional) 
					0x00, // (P2) Bits b8 - b5: '0' = standard display text No. 1, 'F' = no display message; Bits b4 - b1: '0' = no response data '1' = entire ATR '2' = only historical characters
					0x01, // (Lc) Empty or length of data field 
					waitingPeriodInSeconds, // (Data field) Empty or waiting period in seconds or TLV with the tags: '50' = display text coded as IA5 '80' = waiting period in seconds (coded integer) 
					0x00  // (Le) Empty or '00'
				})
				.CheckStatusBytes(new Dictionary<string,string>
				{
					{ "9000", "Synchronous ICC presented, reset successful" },
					{ "9001", "Asynchronous ICC presented, reset successful" },
					{ "6200", "Warning: no card presented within specified time" },
					{ "6201", "Warning: ICC already present and activated" },
					{ "6400", "Error: Reset not successful" },
					{ "6401", "Error: Process aborted by pressing of cancel key" },
					{ "6900", "Error: Command with timer not supported" },
				});
		}


		/// <summary> 
		/// Beendet einen Auslesevorgang und wirft die Chipkarte aus.
		/// Es wird eine Meldung angezeigt, die zum Entfernen der Karte auffordert, deren Anzeigezeit durch den Timeout-Parameter definiert werden kann. 
		/// </summary>
		/// <param name="waitingPeriodInSeconds">Zeitraum (in Sekunden), der gewartet wird, bis die eingesteckte Chipkarte entnommen wurde.</param>
		public byte[] EjectICC(byte waitingPeriodInSeconds = 0)
		{
			return this.ExecuteCommand(
				sad: 2, // source = Host
				dad: 1, // destination = Card Terminal
				command: new byte[]
				{ 
					// EJECT ICC1 (Karte auswerfen)
					0x20, // CLA '20'
					0x15, // INS '15'
					0x01, // (P1) Device ('01' = ICC1, '02' = ICC2)
					0x00, // (P2) '00'  = standard display text No. 2; 'F0' = no display message
					0x01, // (Lc) 0, 1 or length of data field 
					waitingPeriodInSeconds, // (Parameter) If available, 1 byte will state the timeout until removal of the card or TLV structure '50' = display text coded as IA5 (limited set of characters) '80' =  waiting period in seconds (coded integer)
					// (Le) empty
				})
				.CheckStatusBytes(new Dictionary<string,string>
				{
					{ "9000", "Command successful" },
					{ "9001", "Command successful, card removed" },
					{ "6200", "Warning: Card not removed within specified time" },
				});
		}


		/// <summary>
		/// Wählt den Container mit den eGK-Daten für folgende Auslesevorgänge aus.
		/// </summary>
		public byte[] SelectEGK()
		{
			// eGK-Anwendung selektieren
			return this.ExecuteCommand(
				sad: 2, // source = Host
				dad: 0, // destination = Card
				command: new byte[]
				{
					// SELECT FILE (HCA) (Health Care Application)
					0x00, // CLA '00'
					0xa4, // INS 'A4'
					0x04, // (P1) selectionMode = Ordnerselektion mit applicationIdentifier
					0x0c, // (P2) fileOccurrence + responseType = first occurrence, keine Antwortdaten
					0x06, // (Lc) lenght of data field
					0xd2,0x76,0x00,0x00,0x01,0x02 // (Data field) File ID HCA 'D27600000102'
					// (Le) Empty or length of the expected response
				})
				.CheckStatusBytes(CardTerminalClient.selectFileStatusBytes);
		}


		/// <summary>
		/// Wählt den Container mit den KVK-Daten für folgende Auslesevorgänge aus.
		/// </summary>
		public byte[] SelectKVK()
		{
			// KVK-Applikation selektieren
			return this.ExecuteCommand(
				sad: 2, // source = Host
				dad: 0, // destination = Card
				command: new byte[]
				{
					// SELECT FILE (KVK)
					0x00, // CLA '00'
					0xa4, // INS 'A4'
					0x04, // (P1) selectionMode = Ordnerselektion mit applicationIdentifier
					0x00, // (P2)
					0x06, // (Lc) length of data field
					0xd2,0x76,0x00,0x00,0x01,0x01 // (Data field) File ID KVK 'D27600000101'
					// (Le) Empty or length of the expected response
				})
				.CheckStatusBytes(CardTerminalClient.selectFileStatusBytes);
		}


		// mit dem Select-Kommando kann eine Applikation, ein Ordner oder eine Datei durch ihren/seinen Bezeichner (AID/FID) selektiert werden.
		private static readonly Dictionary<string,string> selectFileStatusBytes = new Dictionary<string,string>
		{
			{ "9000", "Command successful - erfolgreiche Selektion eines Files" },
			{ "6283", "Warning: FileDeactivated - selektiertes File ist logisch oder physikalisch deaktiviert" },
			{ "6a82", "Error: FileNotFound - zu selektierendes File wurde nicht gefunden" },
			{ "6900", "Error: Command not allowed - Mobiles Kartenterminal: Autorisierung fehlt" },
		};


		/// <summary>
		/// Liest die eGK-Datensätze für die Patientendaten und die Versicherungsdaten aus.
		/// </summary>
		public EgkResult ReadEGK()
		{
			// Personendaten (PD) lesen
			byte[] pdData = this.ExecuteCommand(
				sad: 2, // source = Host
				dad: 0, // destination = Card
				command: new byte[]
				 {
					 // READ BINARY (Personal Data) 
					 0x00, // CLA '00'
					 0xb0, // INS 'B0'
					 0x81, // (P1) READ BINARY mit shortFileIdentifier: 128 + shortFileIdentifier, d.h. '80' + shortFileIdentifier (EF.PD = '01') 
					 0x00, // (P2) Offset
					 // (Lc) Empty
					 // (Data field) Empty
					 0x00,0x00,0x00  // (Le) Number of bytes to be read. If Le = 00 or 000000 applies, the file is read through to its end, with Le = 00 having a maximum of 256 bytes. 
				})
				.CheckStatusBytes(CardTerminalClient.readBinaryStatusBytes);

			// allgemeine Versicherungsdaten (VD) lesen
			byte[] vdData = this.ExecuteCommand(
				sad: 2, // source = Host
				dad: 0, // destination = Card
				command: new byte[]
				{
					// READ BINARY (Insurance Data)
					0x00, // CLA '00'
					0xb0, // INS 'B0'
					0x82, // (P1) READ BINARY mit shortFileIdentifier: 128 + shortFileIdentifier, d.h. '80' + shortFileIdentifier (EF.VD = '02') 
					0x00, // (P2) Offset
					// (Lc) Empty
					// (Data field) Empty
					0x00,0x00,0x00  // (Le) Number of bytes to be read. If Le = 00 or 000000 applies, the file is read through to its end, with Le = 00 having a maximum of 256 bytes. 
				})
				.CheckStatusBytes(CardTerminalClient.readBinaryStatusBytes);

			return new EgkResult(pdData,vdData);
		}


		/// <summary>
		/// Liest den KVK-Datensatz aus.
		/// </summary>
		public KvkResult ReadKVK()
		{
			// KVK-Template lesen
			byte[] kvkData = this.ExecuteCommand(
				sad: 2, // source = Host
				dad: 0, // destination = Card
				command: new byte[]
				{ 
					// READ BINARY (KVK)
					0x00, // CLA '00'
					0xb0, // INS 'B0'
					0x00, // (P1)
					0x00, // (P2)
					0x00  // (Le)
				})
				.CheckStatusBytes(CardTerminalClient.readBinaryStatusBytes);

			return new KvkResult(kvkData);
		}


		// Read Binary liest die gewünschte Anzahl von Bytes aus einer zuvor selektierten Datei.
		// Read Binary mit Short File Identifier (SFID) kombiniert die Selektion einer Datei mit dem Lesen aus dieser Datei.
		private static readonly Dictionary<string,string> readBinaryStatusBytes = new Dictionary<string,string>
		{
			{ "9000", "Command successful - Datei gelesen" },
			{ "6282", "Warning: EndOfFileWarning - weniger Daten vorhanden, als mittels Ne angefordert" },
			{ "6281", "Warning: CorruptDataWarning - möglicherweise sind die Antwortdaten korrupt" },
			{ "6700", "Error: WrongLength - Die Anzahl der angeforderten Daten übersteigt die maximale Puffergröße." },
			{ "6a82", "Error: FileNotFound - per shortFileIdentifier adressiertes EF nicht gefunden" },
			{ "6986", "Error: NoCurrentEF - es ist kein EF ausgewählt" },
			{ "6982", "Error: SecurityStatusNotSatisfied - Zugriffsregel nicht erfüllt" },
			{ "6981", "Error: WrongFileType - ausgewähltes EF ist nicht transparent" },
			{ "6b00", "Error: OffsetTooBig - Parameter offset in Kommando APDU ist zu groß" },
			{ "6900", "Error: Command not allowed - Mobiles Kartenterminal: Autorisierung fehlt" },
		};
	}
}
