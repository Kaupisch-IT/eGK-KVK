using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using KaupischIT.CardReader.Egk.AllgemeineVersicherungsdaten;
using KaupischIT.CardReader.Egk.GeschuetzteVersichertendaten;
using KaupischIT.CardReader.Egk.PersoenlicheVersichertendaten;

namespace KaupischIT.CardReader
{
	/// <summary>
	/// Stellt Daten einer elektronischen Gesundheitskarte (eGK) bereit
	/// </summary>
	public class EgkResult
	{
		/// <summary>
		/// Ruft die Persönlichen Versichertendaten (PD) aus den Versichertenstammdaten einer eGK ab
		/// </summary>
		public PersoenlicheVersichertendaten PersoenlicheVersichertendaten { get; private set; }

		/// <summary>
		/// Ruft die Allgemeinen Versicherungsdaten (VD) aus den Versichertenstammdaten einer eGK ab
		/// </summary>
		public AllgemeineVersicherungsdaten AllgemeineVersicherungsdaten { get; private set; }

		/// <summary>
		/// Ruft die Geschützten Versichertendaten (GVD) aus den Versichertenstammdaten einer eGK ab
		/// </summary>
		public GeschuetzteVersichertendaten GeschuetzteVersichertendaten { get; private set; }


		/// <summary>
		/// Initialisiert eine neue Instanz der EgkResult-Klasse und dekodiert die übergebenen Versichertenstammdaten einer eGK
		/// </summary>
		/// <param name="pdData">die Rohdaten mit den Personendaten (PD)</param>
		/// <param name="vdData">die Rohdaten mit den Allgemeinen Versicherungsdaten (VD) und den Geschützten Versichertendaten (GVD)</param>
		public EgkResult(byte[] pdData,byte[] vdData)
		{
			this.DecodePD(pdData);
			this.DecodeVD(vdData);
		}


		private void DecodePD(byte[] bytes)
		{
			int length = (bytes[0]<<8) + bytes[1]; // die ersten beiden Bytes der Rohdaten beinhalten die Längenangabe für die eigentlichen Nutzdaten

			// Nutzdaten extrahieren...
			byte[] compressedData = new byte[length];
			Array.Copy(bytes,2,compressedData,0,compressedData.Length);

			// ...und dann deserialisieren
			this.PersoenlicheVersichertendaten = this.Decompress<PersoenlicheVersichertendaten>(compressedData);
		}


		private void DecodeVD(byte[] bytes)
		{
			// die Rohdaten beginnen mit jeweils 2 Bytes Offset für Start & Ende der eigentlichen Nutzdaten mit den allgemeinen Versicherungsdaten (VD)... 
			int offsetStartVD = (bytes[0]<<8) + bytes[1];
			int offsetEndVD = (bytes[2]<<8) + bytes[3];
			// ...sowie je 2 Bytes Offset für Start & Ende der eigentlichen Nutzdaten mit den geschützten Versichertendaten (GVD)
			int offsetStartGVD = (bytes[4]<<8) + bytes[5];
			int offsetEndGVD = (bytes[6]<<8) + bytes[7];

			// Nutzdaten mit den allgemeinen Versicherungsdaten (VD) extrahieren und deserialisieren
			byte[] compressedDataVD = new byte[Math.Min(offsetEndVD+1,bytes.Length)-offsetStartVD];
			if (offsetStartVD!=offsetEndVD)
			{
				Array.Copy(bytes,offsetStartVD,compressedDataVD,0,compressedDataVD.Length);
				this.AllgemeineVersicherungsdaten = this.Decompress<AllgemeineVersicherungsdaten>(compressedDataVD);
			}

			// Nutzdaten mit den geschützten Versichertendaten (GVD) extrahieren und deserialisieren
			byte[] compressedDataGVD = new byte[Math.Min(offsetEndGVD+1,bytes.Length)-offsetStartGVD];
			if (offsetStartGVD!=offsetEndGVD)
			{
				Array.Copy(bytes,offsetStartGVD,compressedDataGVD,0,compressedDataGVD.Length);
				this.GeschuetzteVersichertendaten = this.Decompress<GeschuetzteVersichertendaten>(compressedDataGVD);
			}
		}


		/// <summary>
		/// Dekomprimiert und deserialisiert die ZIP-komprimierten XML-Nutzdaten (ISO-8859-15 codiert)
		/// </summary>
		/// <typeparam name="T">Der Objekttyp, den der XmlSerializer serialisieren soll</typeparam>
		private T Decompress<T>(byte[] compressedData)
		{
			// die Daten selbst werden als...
			using (MemoryStream memoryStream = new MemoryStream(compressedData))
			using (GZipStream gzipStream = new GZipStream(memoryStream,CompressionMode.Decompress)) // ...ZIP-Komprimiertes,...
			using (StreamReader streamReader = new StreamReader(gzipStream,Encoding.GetEncoding("ISO-8859-15"))) /// ...ISO-8859-15 codiertes..
			{
				// ...XML-Dokument abgelegt
				string xmlContent = streamReader.ReadToEnd(); 
				Debug.WriteLine(xmlContent);

				// XML-Daten gemäß vorgegebenem XML-Schema deserialisieren
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				using (TextReader textReader = new StringReader(xmlContent))
				using (XmlTextReader xmlTextReader = new XmlTextReader(textReader) { Namespaces = false }) // die verschiedenen Schema-Versionen (5.1, 5.2) haben unterschiedliche XML-Namespaces - diese hier ignorieren
					return (T)xmlSerializer.Deserialize(xmlTextReader);
			}
		}
	}
}
