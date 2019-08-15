using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CardReader.Results
{
	[DebuggerDisplay("{VorName} {FamilienName}, {KrankenKassenName}")]
	public class KvkResult
	{
		private readonly Dictionary<byte,string> values;

		public KvkResult(byte[] bytes)
		{
			this.values = this.Decode(bytes);
		}


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


		public string this[byte tag]
		{
			get
			{
				string result;
				return (this.values.TryGetValue(tag,out result)) ? result : null;
			}
		}


		public string KrankenKassenName
		{ get { return this[0x80]; } }

		public string KrankenKassenNummer
		{ get { return this[0x81]; } }

		public string VertragskassenNummer
		{ get { return this[0x8F]; } }

		public string VersichertenNummer
		{ get { return this[0x82]; } }

		public string VersichertenStatus
		{ get { return this[0x83]; } }

		public string StatusErgänzung
		{ get { return this[0x90]; } }

		public string Titel
		{ get { return this[0x84]; } }

		public string VorName
		{ get { return this[0x85]; } }

		public string NamensZusatz_VorsatzWort
		{ get { return this[0x86]; } }

		public string FamilienName
		{ get { return this[0x87]; } }

		public string GeburtsDatum
		{ get { return this[0x88]; } }

		public string StraßenName_HausNummer
		{ get { return this[0x89]; } }

		public string WohnsitzLänderCode
		{ get { return this[0x8A]; } }

		public string Postleitzahl
		{ get { return this[0x8B]; } }

		public string OrtsName
		{ get { return this[0x8C]; } }

		public string GültigkeitsDatum
		{ get { return this[0x8D]; } }
	}
}
