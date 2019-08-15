using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using CardReader.Results.Egk.AllgemeineVersicherungsdaten;
using CardReader.Results.Egk.GeschuetzteVersichertendaten;
using CardReader.Results.Egk.PersoenlicheVersichertendaten;

namespace CardReader.Results
{
	public class EgkResult
	{
		public IPersoenlicheVersichertendaten PersoenlicheVersichertendaten { get; private set; }
		public IAllgemeineVersicherungsdaten AllgemeineVersicherungsdaten { get; private set; }
		public IGeschuetzteVersichertendaten GeschuetzteVersichertendaten { get; private set; }


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

			this.PersoenlicheVersichertendaten = this.Decompress<IPersoenlicheVersichertendaten>(compressedData,new Dictionary<string,Type>
			{
				{  "5.1.0", typeof(PersoenlicheVersichertendaten51) },
				{  "5.2.0", typeof(PersoenlicheVersichertendaten52) },
			});
		}


		private void DecodeVD(byte[] bytes)
		{
			int offsetStartVD = (bytes[0]<<8) + bytes[1];
			int offsetEndVD = (bytes[2]<<8) + bytes[3];
			int offsetStartGVD = (bytes[4]<<8) + bytes[5];
			int offsetEndGVD = (bytes[6]<<8) + bytes[7];

			byte[] compressedDataVD = new byte[offsetEndVD-offsetStartVD];
			Array.Copy(bytes,offsetStartVD,compressedDataVD,0,compressedDataVD.Length);
			this.AllgemeineVersicherungsdaten = this.Decompress<IAllgemeineVersicherungsdaten>(compressedDataVD,new Dictionary<string,Type>
			{
				{  "5.1.0", typeof(AllgemeineVersicherungsdaten51) },
				{  "5.2.0", typeof(AllgemeineVersicherungsdaten52) },
			});
#if false
			byte[] compressedDataGVD = new byte[offsetEndGVD-offsetStartGVD];
			Array.Copy(bytes,offsetStartGVD,compressedDataGVD,0,compressedDataGVD.Length);
			this.GeschuetzteVersichertendaten = this.Decompress<IGeschuetzteVersichertendaten>(compressedDataGVD,new Dictionary<string,Type>
			{
				{  "5.1.0", typeof(GeschuetzteVersichertendaten51) },
				{  "5.2.0", typeof(GeschuetzteVersichertendaten52) },
			});
#endif
		}


		private T Decompress<T>(byte[] compressedData,Dictionary<string,Type> actualTypeMapping)
		{
			using (MemoryStream memoryStream = new MemoryStream(compressedData))
			using (GZipStream gzipStream = new GZipStream(memoryStream,CompressionMode.Decompress))
			using (StreamReader streamReader = new StreamReader(gzipStream,Encoding.GetEncoding("ISO-8859-15")))
			{
				string xmlContent = streamReader.ReadToEnd();
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xmlContent);

				string version = xmlDocument.SelectSingleNode("//@CDM_VERSION").Value;
				if (!actualTypeMapping.ContainsKey(version))
					throw new NotImplementedException(version);

				XmlSerializer xmlSerializer = new XmlSerializer(actualTypeMapping[version]);
				using (TextReader textReader = new StringReader(xmlContent))
					return (T)xmlSerializer.Deserialize(textReader);
			}
		}
	}
}
