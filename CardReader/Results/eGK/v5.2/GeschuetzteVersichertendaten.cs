using System;
using System.Xml.Serialization;
using CardReader.Results.Egk.GeschuetzteVersichertendaten;

namespace CardReader.Results.Egk.GeschuetzteVersichertendaten
{
	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	[XmlRoot("UC_GeschuetzteVersichertendatenXML",Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2",IsNullable = false)]
	public class GeschuetzteVersichertendaten52 : IGeschuetzteVersichertendaten
	{
		[XmlElement("Zuzahlungsstatus")]
		public Zuzahlungsstatus52 Zuzahlungsstatus { get; set; }

		[XmlElement("BesonderePersonengruppe",DataType = "integer")]
		public string BesonderePersonengruppe { get; set; }

		[XmlElement("DMP_Kennzeichnung",DataType = "integer")]
		public string DMP_Kennzeichnung { get; set; }

		[XmlElement("Selektivvertraege")]
		public Selektivvertraege52 Selektivvertraege { get; set; }

		[XmlElement("RuhenderLeistungsanspruch")]
		public RuhenderLeistungsanspruch52 RuhenderLeistungsanspruch { get; set; }

		[XmlAttribute("CDM_VERSION")]
		public string CDM_VERSION { get; set; }


		IZuzahlungsstatus IGeschuetzteVersichertendaten.Zuzahlungsstatus { get { return this.Zuzahlungsstatus; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class Zuzahlungsstatus52 : IZuzahlungsstatus
	{
		[XmlElement("Status",DataType = "integer")]
		public string Status { get; set; }

		[XmlElement("Gueltig_bis")]
		public string Gueltig_bis { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class Selektivvertraege52
	{
		[XmlElement("Aerztlich",DataType = "integer")]
		public string Aerztlich { get; set; }

		[XmlElement("Zahnaerztlich",DataType = "integer")]
		public string Zahnaerztlich { get; set; }

		[XmlElement("Art")]
		public string Art { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class RuhenderLeistungsanspruch52
	{
		[XmlElement("Beginn")]
		public string Beginn { get; set; }

		[XmlElement("Ende")]
		public string Ende { get; set; }

		[XmlElement("ArtDesRuhens",DataType = "integer")]
		public string ArtDesRuhens { get; set; }
	}
}