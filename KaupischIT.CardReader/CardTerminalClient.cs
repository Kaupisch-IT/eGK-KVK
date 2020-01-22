using System;
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
				.ExpectStatusBytes("9000","9500");
		}


		/// <summary> Kartenanforderung </summary>
		public void RequestICC()
		{
			this.ExecuteCommand(sad: 2,dad: 1,command: new byte[] { 0x20,0x12,0x01,0x00,0x00 })
				.ExpectStatusBytes("9000","9001","6200","6201").GetStatusBytes();
		}

		/// <summary> Karte auswerfen </summary>
		public void EjectICC()
		{
			this.ExecuteCommand(sad: 2,dad: 1,command: new byte[] { 0x20,0x15,0x01,0x00 })
				.ExpectStatusBytes("9000","9001","6200");
		}


		/// <summary> eGGK-Applikation selektieren </summary>
		public void SelectEGK()
		{
			this.ExecuteCommand(sad: 2,dad: 0,command: new byte[] { 0x00,0xa4,0x04,0x0c,0x06,0xd2,0x76,0x00,0x00,0x01,0x02 })
				.ExpectStatusBytes("9000");
		}


		/// <summary> VST-Template und VD-Template lesen </summary>
		public EgkResult ReadEGK()
		{
			byte[] pdData = this.ExecuteCommand(sad: 2,dad: 0,command: new byte[] { 0x00,0xb0,0x81,0x00,0x00,0x00,0x00 })
				.ExpectStatusBytes("9000","6282");

			byte[] vdData = this.ExecuteCommand(sad: 2,dad: 0,command: new byte[] { 0x00,0xb0,0x82,0x00,0x00,0x00,0x00 })
				.ExpectStatusBytes("9000","6282","6f00");

			return new EgkResult(pdData,vdData);
		}


		/// <summary> KVK-Applikation selektieren </summary>
		public void SelectKVK()
		{
			this.ExecuteCommand(sad: 2,dad: 0,command: new byte[] { 0x00,0xa4,0x04,0x00,0x06,0xd2,0x76,0x00,0x00,0x01,0x01 })
				.ExpectStatusBytes("9000");
		}

		/// <summary> KVK-Template lesen </summary>
		public KvkResult ReadKVK()
		{
			byte[] kvkData = this.ExecuteCommand(sad: 2,dad: 0,command: new byte[] { 0x00,0xb0,0x00,0x00,0x00 })
				.ExpectStatusBytes("9000","6282");

			return new KvkResult(kvkData);
		}
	}
}
