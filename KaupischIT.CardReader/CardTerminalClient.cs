using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KaupischIT.CardReader
{
	/// <summary>
	/// Stellt Funktionen zum Zugriff auf die Dateien einer elektronischen Gesundheitskarte (eGK) oder Krankenversichertenkarte (KVK) bzw. Card für Privatversicherte (PKV-Card) bereit
	/// </summary>
	[DebuggerStepThrough]
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
		/// Sendet ein Kommando an ein Kartenterminal/eine Chipkarte und gibt die Antwort zurück
		/// </summary>
		/// <param name="dad">Destination Address - Empfänger des Kommandos</param>
		/// <param name="sad">Source Address - Sender des Kommandos</param>
		/// <param name="command">Chipkarten- bzw. Kartenterminal-Kommando</param>
		/// <returns>eine Bytefolge mit der Antwort auf das Kommando</returns>
		public byte[] ExecuteCommand(byte dad,byte sad,byte[] command)
		{
			ushort responseLength = ushort.MaxValue;
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
		/// Versetzt das Gerät in einen definierten Grundzustand
		/// </summary>
		public void ResetCT()
		{
			this.ExecuteCommand(
				sad: 2, // source = Host
				dad: 1, // destination = Card Terminal
				command: new byte[]
				{ 
					// RESET CT (Kartenleser zurücksetzen)
					0x20,0x11,0x00,0x00,0x00
				})
				.CheckStatusBytes(new Dictionary<string,string>
				{
					{ "9000", "Reset successful" },
					{ "9001", "Asynchronous ICC, reset successful" },
					{ "6400", "Reset not successful" },
				});
		}

		/// <summary> 
		/// Fordert eine eingelegte Chipkarte an und leitet folgende Auslesevorgänge ein
		/// </summary>
		public string RequestICC()
		{
			return this.ExecuteCommand(
				sad: 2, // source = Host
				dad: 1, // destination = Card Terminal
				command: new byte[]
				{ 
					// REQUEST ICC1 (Kartenanforderung)
					0x20,0x12,0x01,0x00,0x00
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
				})
				.GetStatusBytes();
		}

		/// <summary> 
		/// Beendet einen Auslesevorgang und wirft die Chipkarte aus
		/// </summary>
		public string EjectICC()
		{
			return this.ExecuteCommand(
				sad: 2, // source = Host
				dad: 1, // destination = Card Terminal
				command: new byte[]
				{ 
					// EJECT ICC1 (Karte auswerfen)
					0x20,0x15,0x01,0x00
				})
				.CheckStatusBytes(new Dictionary<string,string>
				{
					{ "9000", "Command successful" },
					{ "9001", "Command successful, card removed" },
					{ "6200", "Warning: Card not removed within specified time" },
				})
				.GetStatusBytes();
		}


		/// <summary>
		/// Wählt den Container mit den eGK-Daten für folgende Auslesevorgänge aus.
		/// </summary>
		public string SelectEGK()
		{
			// eGK-Anwendung selektieren
			return this.ExecuteCommand(
				sad: 2, // source = Host
				dad: 0, // destination = Card
				command: new byte[]
				{
					// SELECT FILE (HCA) (Health Care Application)
					0x00,0xa4,0x04,0x0c,0x06,
					0xd2,0x76,0x00,0x00,0x01,0x02
				})
				.CheckStatusBytes(selectFileStatusBytes)
				.GetStatusBytes();
		}

		/// <summary>
		/// Wählt den Container mit den KVK-Daten für folgende Auslesevorgänge aus.
		/// </summary>
		public string SelectKVK()
		{
			// KVK-Applikation selektieren
			return this.ExecuteCommand(
				sad: 2, // source = Host
				dad: 0, // destination = Card
				command: new byte[]
				{
					// SELECT FILE (KVK)
					0x00,0xa4,0x04,0x00,0x06,0xd2,0x76,0x00,0x00,0x01,0x01
				})
				.CheckStatusBytes(selectFileStatusBytes)
				.GetStatusBytes();
		}

		// mit dem Select-Kommando kann eine Applikation, ein Ordner oder eine Datei durch ihren/seinen Bezeichner (AID/FID) selektiert werden.
		private static readonly Dictionary<string,string> selectFileStatusBytes = new Dictionary<string,string>
		{
			{ "9000", "Command successful erfolgreiche Selektion eines Files" },
			{ "6283", "Warning: FileDeactivated selektiertes File ist logisch oder physikalisch deaktiviert" },
			{ "6a82", "Error: FileNotFound zu selektierendes File wurde nicht gefunden" },
			{ "6900", "Error: Command not allowed Mobiles Kartenterminal: Autorisierung fehlt" },
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
					0x00,0xb0,0x81,0x00,0x00,0x00,0x00
				})
				.CheckStatusBytes(readBinaryStatusBytes);

			// allgemeine Versicherungsdaten (VD) lesen
			byte[] vdData = this.ExecuteCommand(
				sad: 2, // source = Host
				dad: 0, // destination = Card
				command: new byte[]
				{ 
					// READ BINARY (Insurance Data)
					0x00,0xb0,0x82,0x00,0x00,0x00,0x00
				})
				.CheckStatusBytes(readBinaryStatusBytes);

			// (Bei den gelesenen Daten handelt es sich um gezippte XML-Dateien)
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
					0x00,0xb0,0x00,0x00,0x00
				})
				.CheckStatusBytes(readBinaryStatusBytes);

			return new KvkResult(kvkData);
		}

		// Read Binary liest die gewünschte Anzahl von Bytes aus einer zuvor selektierten Datei.
		// Read Binary mit Short File Identifier (SFID) kombiniert die Selektion einer Datei mit dem Lesen aus dieser Datei.
		private static readonly Dictionary<string,string> readBinaryStatusBytes = new Dictionary<string,string>
		{
			{ "9000", "Command successful" },
			{ "6282", "Warning: EndOfFileWarning weniger Daten vorhanden, als mittels Ne angefordert" },
			{ "6281", "Warning: CorruptDataWarning möglicherweise sind die Antwortdaten korrupt" },
			{ "6700", "Error: WrongLength Die Anzahl der angeforderten Daten übersteigt die maximale Puffergröße." },
			{ "6a82", "Error: FileNotFound per shortFileIdentifier adressiertes EF nicht gefunden" },
			{ "6986", "Error: NoCurrentEF es ist kein EF ausgewählt" },
			{ "6982", "Error: SecurityStatusNotSatisfied Zugriffsregel nicht erfüllt" },
			{ "6981", "Error: WrongFileType ausgewähltes EF ist nicht transparent" },
			{ "6b00", "Error: OffsetTooBig Parameter offset in Kommando APDU ist zu groß" },
			{ "6900", "Error: Command not allowed Mobiles Kartenterminal: Autorisierung fehlt" },
		};
	}
}
