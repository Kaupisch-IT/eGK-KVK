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


		public byte[] ExecuteCommand(CtCommand command)
		{
			ushort responseLength = ushort.MaxValue;
			byte[] response = new byte[responseLength];

			byte sourceAddress = 2;
			byte destinationAddress = command.DestinationAddress;
			byte[] bytecode = command.Bytecode;

			this.cardTerminalApi.Data(this.terminalID,ref destinationAddress,ref sourceAddress,(ushort)bytecode.Length,ref bytecode[0],ref responseLength,ref response[0]);

			byte[] result = new byte[responseLength];
			Array.Copy(response,result,responseLength);
			return result;
		}

		public void Dispose()
		{
			this.cardTerminalApi.Close(this.terminalID);
			this.cardTerminalApi.Dispose();
		}



		public void ResetCT()
		{
			this.ExecuteCommand(CtCommand.ResetCT).ExpectStatusBytes("9000","9500");
		}


		public string RequestICC()
		{
			return this.ExecuteCommand(CtCommand.RequestICC).ExpectStatusBytes("9000","9001","6200","6201").GetStatusBytes();
		}


		public void SelectEGK()
		{
			this.ExecuteCommand(CtCommand.SelectEGK).ExpectStatusBytes("9000");
		}


		public EgkResult ReadEGK()
		{
			byte[] pdData = this.ExecuteCommand(CtCommand.ReadPD).ExpectStatusBytes("9000","6282");
			byte[] vdData = this.ExecuteCommand(CtCommand.ReadVD).ExpectStatusBytes("9000","6282","6f00");
			return new EgkResult(pdData,vdData);
		}


		public void SelectKVK()
		{
			this.ExecuteCommand(CtCommand.GetStatusCtmdo).ExpectStatusBytes("9000");
		}

		public KvkResult ReadKVK()
		{
			var bytes = this.ExecuteCommand(CtCommand.ReadKVK).ExpectStatusBytes("9000","6282");
			return new KvkResult(bytes);
		}


		public void EjectICC()
		{
			this.ExecuteCommand(CtCommand.EjectICC).ExpectStatusBytes("9000","9001","6200");
		}
	}
}
