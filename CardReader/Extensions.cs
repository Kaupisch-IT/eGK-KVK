using System;
using System.Linq;
using System.Diagnostics;

namespace CardReader
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
		
		[DebuggerStepThrough]
		public static byte[] ExpectStatusBytes(this byte[] bytes,params string[] hexReturnCodes)
		{
			string actual = bytes.GetStatusBytes();
#if DEBUG
			string methodName = new StackTrace().GetFrame(1).GetMethod().Name;
			Debug.WriteLine(methodName.PadRight(12)+" "+actual+" ("+String.Join(", ",hexReturnCodes)+")");
#endif			
			if (!hexReturnCodes.Contains(actual))
				throw new Exception("Tatsächlicher Rückgabewert: "+actual+"; Erwartet: "+String.Join(", ",hexReturnCodes));

			return bytes;
		}
	}
}
