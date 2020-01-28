using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KaupischIT.CardReader
{
	[DebuggerStepThrough]
	public sealed class CardTerminalClient : IDisposable
	{
		private readonly CtApi cardTerminalApi;
		private readonly ushort terminalID;
		private readonly ushort portNumber;


		public CardTerminalClient(string path,ushort portNumber = 1,ushort terminalID = 1)
		{
			this.cardTerminalApi = new CtApi(path);
			this.portNumber = portNumber;
			this.terminalID = terminalID;

			this.cardTerminalApi.Init(this.terminalID,this.portNumber);
		}


		public byte[] ExecuteCommand(byte dad,byte sad,byte[] command)
		{
			ushort responseLength = ushort.MaxValue;
			byte[] response = new byte[responseLength];

			this.cardTerminalApi.Data(this.terminalID,ref dad,ref sad,(ushort)command.Length,ref command[0],ref responseLength,ref response[0]);

			byte[] result = new byte[responseLength];
			Array.Copy(response,result,responseLength);
			return result;
		}

		public void Dispose()
		{
			this.cardTerminalApi.Close(this.terminalID);
			this.cardTerminalApi.Dispose();
		}



		/// <summary> Kartenleser zurücksetzen </summary>
		public void ResetCT()
		{
			this.ExecuteCommand(sad: 2,dad: 1,command: new byte[] { 0x20,0x11,0x00,0x00,0x00 })
				.CheckStatusBytes(new Dictionary<string,string>
				{
					{ "9000", "Reset successful" },
					{ "9001", "Asynchronous ICC, reset successful" },
					{ "6400", "Reset not successful" },
				});
		}

		/// <summary> 
		/// Kartenanforderung 
		/// </summary>
		public string RequestICC()
		{
			return this.ExecuteCommand(sad: 2,dad: 1,command: new byte[] { 0x20,0x12,0x01,0x00,0x00 })
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

		/// <summary> Karte auswerfen </summary>
		public string EjectICC()
		{
			return this.ExecuteCommand(sad: 2,dad: 1,command: new byte[] { 0x20,0x15,0x01,0x00 })
				.CheckStatusBytes(new Dictionary<string,string>
				{
					{ "9000", "Command successful" },
					{ "9001", "Command successful, card removed" },
					{ "6200", "Warning: Card not removed within specified time" },
				})
				.GetStatusBytes();
		}


		/// <summary> Selektieren der eGK-Chipkartenanwendung </summary>
		public string SelectEGK()
		{
			return this.ExecuteCommand(sad: 2,dad: 0,command: new byte[] { 0x00,0xa4,0x04,0x0c,0x06,0xd2,0x76,0x00,0x00,0x01,0x02 })
				.CheckStatusBytes(selectFileStatusBytes)
				.GetStatusBytes();
		}

		/// <summary> Selektieren der KVK-Chipkartenanwendung </summary>
		public string SelectKVK()
		{
			return this.ExecuteCommand(sad: 2,dad: 0,command: new byte[] { 0x00,0xa4,0x04,0x00,0x06,0xd2,0x76,0x00,0x00,0x01,0x01 })
				.CheckStatusBytes(selectFileStatusBytes)
				.GetStatusBytes();
		}

		private static readonly Dictionary<string,string> selectFileStatusBytes = new Dictionary<string,string>
		{
			{ "9000", "Command successful erfolgreiche Selektion eines Files" },
			{ "6283", "Warning: FileDeactivated selektiertes File ist logisch oder physikalisch deaktiviert" },
			{ "6a82", "Error: FileNotFound zu selektierendes File wurde nicht gefunden" },
			{ "6900", "Error: Command not allowed Mobiles Kartenterminal: Autorisierung fehlt" },
		};


		/// <summary> Lesen des VST- und VD-Datenbereichs </summary>
		public EgkResult ReadEGK()
		{
			byte[] pdData = this.ExecuteCommand(sad: 2,dad: 0,command: new byte[] { 0x00,0xb0,0x81,0x00,0x00,0x00,0x00 })
				.CheckStatusBytes(readBinaryStatusBytes);

			byte[] vdData = this.ExecuteCommand(sad: 2,dad: 0,command: new byte[] { 0x00,0xb0,0x82,0x00,0x00,0x00,0x00 })
				.CheckStatusBytes(readBinaryStatusBytes);

			return new EgkResult(pdData,vdData);
		}

		/// <summary> Lesen des VST- und VD-Datenbereichs </summary>
		public KvkResult ReadKVK()
		{
			byte[] kvkData = this.ExecuteCommand(sad: 2,dad: 0,command: new byte[] { 0x00,0xb0,0x00,0x00,0x00 })
				.CheckStatusBytes(readBinaryStatusBytes);

			return new KvkResult(kvkData);
		}

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
