using System;
using System.Linq;
using System.Diagnostics;

namespace KaupischIT.CardReader
{
	public static class Extensions
	{
		public static string ToHexString(this byte value)
		{
			return Convert.ToString(value,16).PadLeft(2,'0');
		}

		public static string ToBinaryString(this byte value)
		{
			return Convert.ToString(value,2).PadLeft(8,'0');
		}

		public static string GetStatusBytes(this byte[] bytes)
		{
			if (bytes==null || bytes.Length<2)
				return null;
			else
				return bytes[bytes.Length-2].ToHexString()+bytes[bytes.Length-1].ToHexString();
		}

		public static byte[] ExpectStatusBytes(this byte[] bytes,params string[] hexReturnCodes)
		{
#if DEBUG
			string actual = bytes.GetStatusBytes();
			string methodName = new StackTrace().GetFrame(1).GetMethod().Name;
			bool isWarning = !hexReturnCodes.Contains(actual);

			Debug.WriteLine(methodName.PadRight(12)+" "+actual+" (expected: "+String.Join(", ",hexReturnCodes)+")",(isWarning) ? "Warning" : null);
#endif
			return bytes;
		}
	}
}
