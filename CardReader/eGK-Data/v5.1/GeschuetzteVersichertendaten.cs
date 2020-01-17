using System;
using System.Xml.Serialization;

namespace CardReader.Egk.GeschuetzteVersichertendaten
{
	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_GeschuetzteVersichertendatenXML/v5.1")]
	[XmlRoot("UC_GeschuetzteVersichertendatenXML",Namespace = "http://ws.gematik.de/fa/vsds/UC_GeschuetzteVersichertendatenXML/v5.1",IsNullable = false)]
	public class GeschuetzteVersichertendaten51 : IGeschuetzteVersichertendaten
	{
		/// <summary>
		/// Gibt an, ob für den Versicherten eine Befreiung nach § 62 SGB V vorliegt.
		/// Dieses Datenfeld ist besonders schützenswert und daher nicht frei auslesbar, sondern nur berechtigten Personen zugänglich.
		/// </summary>
		[XmlElement("Zuzahlungsstatus")]
		public Zuzahlungsstatus51 Zuzahlungsstatus { get; set; }

		/// <summary> Gibt die Zugehörigkeit des Versicherten zu einer besonderen Personengruppe an. Die Kennzeichnung erfolgt gemäß der Schlüsseltabelle. </summary>
		[XmlElement("Besondere_Personengruppe")]
		public string Besondere_Personengruppe { get; set; }

		/// <summary> Gibt die Teilnahme des Versicherten an einem Disease Management Program an. Die Kennzeichnung erfolgt gemäß der Schlüsseltabelle. </summary>
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
		/// <summary>
		/// Gibt an, ob für den Versicherten eine Befreiung nach § 62 SGB V vorliegt.
		/// Dieses Datenfeld ist besonders schützenswert und daher nicht frei auslesbar, sondern nur berechtigten Personen zugänglich.
		/// </summary>
		[XmlElement("Status",DataType = "integer")]
		public string Status { get; set; }

		/// <summary>
		/// Gibt die Gültigkeit der Befreiung von der Zuzahlungspflicht nach § 62 SGB V an. Format: YYYYMMDD
		/// Dieses Datenfeld ist besonders schützenswert und daher nicht frei auslesbar, sondern nur berechtigten Personen zugänglich.
		/// </summary>
		[XmlElement("Gueltig_bis")]
		public string Gueltig_bis { get; set; }
	}
}