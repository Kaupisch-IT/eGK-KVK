using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using CardReader.Egk.AllgemeineVersicherungsdaten;
using CardReader.Egk.GeschuetzteVersichertendaten;
using CardReader.Egk.PersoenlicheVersichertendaten;

namespace CardReader
{
	public class EgkResult
	{
		public PersoenlicheVersichertendaten PersoenlicheVersichertendaten { get; private set; }
		public AllgemeineVersicherungsdaten AllgemeineVersicherungsdaten { get; private set; }
		public GeschuetzteVersichertendaten GeschuetzteVersichertendaten { get; private set; }


		public EgkResult(byte[] pdData,byte[] vdData)
		{
			this.DecodePD(pdData);
			this.DecodeVD(vdData);
		}


		private void DecodePD(byte[] bytes)
		{
			int length = (bytes[0]<<8) + bytes[1];

			byte[] compressedData = new byte[length];
			Array.Copy(bytes,2,compressedData,0,compressedData.Length);

			this.PersoenlicheVersichertendaten = this.Decompress<PersoenlicheVersichertendaten>(compressedData);
		}


		private void DecodeVD(byte[] bytes)
		{
			int offsetStartVD = (bytes[0]<<8) + bytes[1];
			int offsetEndVD = (bytes[2]<<8) + bytes[3];
			int offsetStartGVD = (bytes[4]<<8) + bytes[5];
			int offsetEndGVD = (bytes[6]<<8) + bytes[7];

			byte[] compressedDataVD = new byte[offsetEndVD-offsetStartVD];
			if (compressedDataVD.Length>0)
			{
				Array.Copy(bytes,offsetStartVD,compressedDataVD,0,compressedDataVD.Length);
				this.AllgemeineVersicherungsdaten = this.Decompress<AllgemeineVersicherungsdaten>(compressedDataVD);
			}

			byte[] compressedDataGVD = new byte[offsetEndGVD-offsetStartGVD];
			if (compressedDataGVD.Length>0)
			{
				Array.Copy(bytes,offsetStartGVD,compressedDataGVD,0,compressedDataGVD.Length);
				this.GeschuetzteVersichertendaten = this.Decompress<GeschuetzteVersichertendaten>(compressedDataGVD);
			}
		}


		private T Decompress<T>(byte[] compressedData)
		{
			using (MemoryStream memoryStream = new MemoryStream(compressedData))
			using (GZipStream gzipStream = new GZipStream(memoryStream,CompressionMode.Decompress))
			using (StreamReader streamReader = new StreamReader(gzipStream,Encoding.GetEncoding("ISO-8859-15")))
			{
				string xmlContent = streamReader.ReadToEnd();
				Debug.WriteLine(xmlContent);

				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				using (TextReader textReader = new StringReader(xmlContent))
				using (XmlTextReader xmlTextReader = new XmlTextReader(textReader) { Namespaces = false })
					return (T)xmlSerializer.Deserialize(xmlTextReader);
			}
		}
	}
}
