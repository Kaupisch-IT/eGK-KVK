using System;
using System.Xml.Serialization;
using CardReader.Results.Egk.AllgemeineVersicherungsdaten;

namespace CardReader.Results.Egk.AllgemeineVersicherungsdaten
{
	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_AllgemeineVersicherungsdatenXML/v5.1")]
	[XmlRoot("UC_AllgemeineVersicherungsdatenXML",Namespace = "http://ws.gematik.de/fa/vsds/UC_AllgemeineVersicherungsdatenXML/v5.1",IsNullable = false)]
	public class AllgemeineVersicherungsdaten51 : IAllgemeineVersicherungsdaten
	{
		[XmlElement("Versicherter")]
		public Versicherter51 Versicherter { get; set; }

		[XmlAttribute("CDM_VERSION")]
		public string CDM_VERSION { get; set; }


		IVersicherter IAllgemeineVersicherungsdaten.Versicherter { get { return this.Versicherter; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_AllgemeineVersicherungsdatenXML/v5.1")]
	public class Versicherter51 : IVersicherter
	{
		[XmlElement("Versicherungsschutz")]
		public Versicherungsschutz51 Versicherungsschutz { get; set; }

		[XmlElement("Zusatzinfos")]
		public Zusatzinfos51 Zusatzinfos { get; set; }


		IVersicherungsschutz IVersicherter.Versicherungsschutz { get { return this.Versicherungsschutz; } }
		IZusatzinfos IVersicherter.Zusatzinfos { get { return this.Zusatzinfos; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_AllgemeineVersicherungsdatenXML/v5.1")]
	public class Versicherungsschutz51 : IVersicherungsschutz
	{
		[XmlElement("Beginn")]
		public string Beginn { get; set; }

		[XmlElement("Ende")]
		public string Ende { get; set; }

		[XmlElement("Kostentraeger")]
		public Kostentraeger51 Kostentraeger { get; set; }


		IKostentraeger IVersicherungsschutz.Kostentraeger { get { return this.Kostentraeger; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_AllgemeineVersicherungsdatenXML/v5.1")]
	public class Kostentraeger51 : IKostentraeger
	{
		[XmlElement("Kostentraegerkennung",DataType = "integer")]
		public string Kostentraegerkennung { get; set; }

		[XmlElement("Kostentraegerlaendercode")]
		public string Kostentraegerlaendercode { get; set; }

		[XmlElement("Name")]
		public string Name { get; set; }

		[XmlElement("AbrechnenderKostentraeger")]
		public Kostentraeger51 AbrechnenderKostentraeger { get; set; }


		IKostentraeger IKostentraeger.AbrechnenderKostentraeger { get { return this.AbrechnenderKostentraeger; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_AllgemeineVersicherungsdatenXML/v5.1")]
	public class Zusatzinfos51 : IZusatzinfos
	{
		[XmlElement("ZusatzinfosGKV",typeof(ZusatzinfosGKV51))]
		[XmlElement("ZusatzinfosPKV",typeof(ZusatzinfosPKV51))]
		public object Item { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_AllgemeineVersicherungsdatenXML/v5.1")]
	public class ZusatzinfosGKV51 : IZusatzinfosGKV
	{
		[XmlElement("Rechtskreis")]
		public string Rechtskreis { get; set; }

		[XmlElement("Versichertenart")]
		public string Versichertenart { get; set; }

		[XmlElement("Versichertenstatus_RSA")]
		public string Versichertenstatus_RSA { get; set; }

		[XmlElement("Zusatzinfos_Abrechnung_GKV")]
		public Zusatzinfos_Abrechnung_GKV51 Zusatzinfos_Abrechnung_GKV { get; set; }


		IZusatzinfos_Abrechnung_GKV IZusatzinfosGKV.Zusatzinfos_Abrechnung_GKV { get { return this.Zusatzinfos_Abrechnung_GKV; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_AllgemeineVersicherungsdatenXML/v5.1")]
	public class Zusatzinfos_Abrechnung_GKV51 : IZusatzinfos_Abrechnung_GKV
	{
		[XmlElement("Kostenerstattung_ambulant",DataType = "integer")]
		public string Kostenerstattung_ambulant { get; set; }

		[XmlElement("Kostenerstattung_stationaer",DataType = "integer")]
		public string Kostenerstattung_stationaer { get; set; }

		[XmlElement("WOP")]
		public string WOP { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_AllgemeineVersicherungsdatenXML/v5.1")]
	public class ZusatzinfosPKV51 : IZusatzinfosPKV
	{
		[XmlElement("PKV_Verbandstarif")]
		public string PKV_Verbandstarif { get; set; }

		[XmlElement("Beihilfeberechtigung")]
		public Beihilfeberechtigung51 Beihilfeberechtigung { get; set; }

		[XmlElement("StationaereLeistungen")]
		public StationaereLeistungen51[] StationaereLeistungen { get; set; }


		IBeihilfeberechtigung IZusatzinfosPKV.Beihilfeberechtigung { get { return this.Beihilfeberechtigung; } }
		IStationaereLeistungen[] IZusatzinfosPKV.StationaereLeistungen { get { return this.StationaereLeistungen; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_AllgemeineVersicherungsdatenXML/v5.1")]
	public class Beihilfeberechtigung51 : IBeihilfeberechtigung
	{
		[XmlElement("Kennzeichnung")]
		public string Kennzeichnung { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_AllgemeineVersicherungsdatenXML/v5.1")]
	public class StationaereLeistungen51 : IStationaereLeistungen
	{
		[XmlElement("Stationaere_Wahlleistung_Unterkunft")]
		public string Stationaere_Wahlleistung_Unterkunft { get; set; }

		[XmlElement("Prozentwert_Wahlleistung_Unterkunft")]
		public decimal Prozentwert_Wahlleistung_Unterkunft { get; set; }

		[XmlElement("HoechstsatzWahlleistungUnterkunft")]
		public decimal HoechstsatzWahlleistungUnterkunft { get; set; }

		[XmlElement("Stationaere_Wahlleistung_aerztliche_Behandlung",DataType = "integer")]
		public string Stationaere_Wahlleistung_aerztliche_Behandlung { get; set; }

		[XmlElement("Prozentwert_Wahlleistung_aerztliche_Behandlung")]
		public decimal Prozentwert_Wahlleistung_aerztliche_Behandlung { get; set; }

		[XmlElement("Teilnahme_ClinicCard_Verfahren")]
		public string Teilnahme_ClinicCard_Verfahren { get; set; }
	}
}
