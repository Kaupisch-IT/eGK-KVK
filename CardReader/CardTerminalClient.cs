﻿using System;
using CardReader.Commands;
using CardReader.Results;

namespace CardReader
{
	public class CardTerminalClient : IDisposable
	{
		private readonly ICardTerminalApi cardTerminalApi;
		private readonly ushort terminalID;
		private readonly ushort portNumber;


		public CardTerminalClient(ICardTerminalApi cardTerminalApi,ushort portNumber = 1,ushort terminalID = 1)
		{
			this.cardTerminalApi = cardTerminalApi;
			this.portNumber = portNumber;
			this.terminalID = terminalID;

			CtReturnCode returnCode = (CtReturnCode)this.cardTerminalApi.Init(this.terminalID,this.portNumber);
			if (returnCode!=CtReturnCode.OK)
				throw new Exception(returnCode.ToString());
		}


		public void Dispose()
		{
			this.cardTerminalApi.Close(this.terminalID);
		}


		public byte[] ExecuteCommand(Command command)
		{
			ushort responseLength = ushort.MaxValue;
			byte[] response = new byte[responseLength];

			byte sourceAddress = 2;
			byte destinationAddress = command.DestinationAddress;
			byte[] bytecode = command.Bytecode;

			CtReturnCode returnCode = (CtReturnCode)this.cardTerminalApi.Data(this.terminalID,ref destinationAddress,ref sourceAddress,(ushort)bytecode.Length,ref bytecode[0],ref responseLength,ref response[0]);
			if (returnCode==CtReturnCode.OK)
			{
				byte[] result = new byte[responseLength];
				Array.Copy(response,result,responseLength);
				return result;
			}
			else
				throw new Exception(returnCode.ToString());
		}



		public void ResetCT()
		{
			this.ExecuteCommand(CommandSet.ResetCT).ExpectStatusBytes("9000","9500");
		}

		public string RequestICC()
		{
			return this.ExecuteCommand(CommandSet.RequestICC).ExpectStatusBytes("9000","9001").GetStatusBytes();
		}

		public void SelectKVK()
		{
			this.ExecuteCommand(CommandSet.SelectKVK).ExpectStatusBytes("9000");
		}

		public KvkResult ReadKVK()
		{
			var bytes = this.ExecuteCommand(CommandSet.ReadKVK).ExpectStatusBytes("9000","6282");
			return new KvkResult(bytes);
		}


		public void SelectEGK()
		{
			this.ExecuteCommand(CommandSet.SelectEGK).ExpectStatusBytes("9000");
		}


		public EgkResult ReadEGK()
		{
			byte[] pdData = this.ExecuteCommand(CommandSet.ReadPD).ExpectStatusBytes("9000","6282");
			byte[] vdData = this.ExecuteCommand(CommandSet.ReadVD).ExpectStatusBytes("9000","6282"/*,"6f00"*/);
			return new EgkResult(pdData,vdData);  
		}
		

		public void EjectICC()
		{
			this.ExecuteCommand(CommandSet.EjectICC).ExpectStatusBytes("9000");
		}		
	}
}