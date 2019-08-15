﻿using System.Runtime.InteropServices;
namespace CardReader
{
	public class Orga6141Api : ICardTerminalApi
	{
		[DllImport("CTORG32.dll ",EntryPoint = "CT_init")]
		private static extern sbyte CT_init(ushort ctn,ushort pn);

		[DllImport("CTORG32.dll ",EntryPoint = "CT_close")]
		private static extern sbyte CT_close(ushort ctn);

		[DllImport("CTORG32.dll ",EntryPoint = "CT_data")]
		private static extern sbyte CT_data(ushort ctn,ref byte dad,ref byte sad,ushort lenc,ref byte command,ref ushort ulenr,ref byte response);

		public Orga6141Api()
		{

		}

		public sbyte Init(ushort ctn,ushort pn)
		{
			return Orga6141Api.CT_init(ctn,pn);
		}

		public sbyte Close(ushort ctn)
		{
			return Orga6141Api.CT_close(ctn);
		}

		public sbyte Data(ushort ctn,ref byte dad,ref byte sad,ushort lenc,ref byte command,ref ushort ulenr,ref byte response)
		{
			return Orga6141Api.CT_data(ctn,ref dad,ref sad,lenc,ref command,ref ulenr,ref response);
		}
	}
}