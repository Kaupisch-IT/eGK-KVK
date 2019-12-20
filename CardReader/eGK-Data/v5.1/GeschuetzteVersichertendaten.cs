using System;
using System.Xml.Serialization;
using CardReader.Results.Egk.GeschuetzteVersichertendaten;

namespace CardReader.Results.Egk.GeschuetzteVersichertendaten
{
	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_GeschuetzteVersichertendatenXML/v5.1")]
	[XmlRoot("UC_GeschuetzteVersichertendatenXML",Namespace = "http://ws.gematik.de/fa/vsds/UC_GeschuetzteVersichertendatenXML/v5.1",IsNullable = false)]
	public class GeschuetzteVersichertendaten51 : IGeschuetzteVersichertendaten
	{
		[XmlElement("Zuzahlungsstatus")]
		public Zuzahlungsstatus51 Zuzahlungsstatus { get; set; }

		[XmlElement("Besondere_Personengruppe")]
		public string Besondere_Personengruppe { get; set; }

		[XmlElement("DMP_Kennzeichnung")]
		public string DMP_Kennzeichnung { get; set; }

		[XmlAttribute("CDM_VERSION")]
		public string CDM_VERSION { get; set; }


		IZuzahlungsstatus IGeschuetzteVersichertendaten.Zuzahlungsstatus { get { return this.Zuzahlungsstatus; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_GeschuetzteVersichertendatenXML/v5.1")]
	public class Zuzahlungsstatus51 : IZuzahlungsstatus
	{
		[XmlElement("Status",DataType = "integer")]
		public string Status { get; set; }

		[XmlElement("Gueltig_bis")]
		public string Gueltig_bis { get; set; }
	}
}