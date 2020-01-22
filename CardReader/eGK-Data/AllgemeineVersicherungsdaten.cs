using System;
using System.Xml.Serialization;

namespace CardReader.Egk.AllgemeineVersicherungsdaten
{
	[Serializable]
	[XmlType(AnonymousType = true)]
	[XmlRoot("UC_AllgemeineVersicherungsdatenXML",IsNullable = false)]
	public class AllgemeineVersicherungsdaten
	{
		/// <summary> Die Versicherten-ID ist der 10-stellige unveränderliche Teil der 30-stelligen Krankenversichertennummer. </summary>
		[XmlElement("Versicherter")]
		public Versicherter Versicherter { get; set; }

		[XmlAttribute("CDM_VERSION")]
		public string CDM_VERSION { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true)]
	public class Versicherter
	{
		/// <summary>
		/// Gibt den Beginn des Versicherungsschutzes (hier: Leistungsanspruch) des Versicherten bei dem unter Klasse Kostenträger angegebenen Kostenträger an. 
		/// Format: YYYYMMDD (ISO-8601)
		/// </summary>
		[XmlElement("Versicherungsschutz")]
		public Versicherungsschutz Versicherungsschutz { get; set; }

		/// <summary>
		/// Das Informationsmodell VSD beinhaltet Daten für GKV und PKV.
		/// Für die Abbildung des definitiven Datensatzes der VSD wird aber nur die jeweils zutreffende Klasse GKV oder PKV realisiert, d.h. es erfolgte keine Abbildung von PKV-Informationen in den VSD der GKV und umgekehrt.
		/// </summary>
		[XmlElement("Zusatzinfos")]
		public Zusatzinfos Zusatzinfos { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true)]
	public class Versicherungsschutz
	{
		/// <summary>
		/// Gibt den Beginn des Versicherungsschutzes (hier: Leistungsanspruch) des Versicherten bei dem unter Klasse Kostenträger angegebenen Kostenträger an. 
		/// Format: YYYYMMDD (ISO-8601)
		/// </summary>
		[XmlElement("Beginn")]
		public string Beginn { get; set; }

		/// <summary>
		/// Gibt das Ende des Versicherungsschutzes (hier: Leistungsanspruch) des Versicherten bei dem unter Klasse Kostenträger angegebenen Kostenträger an. 
		/// Format: YYYYMMDD (ISO-8601)
		/// </summary>
		[XmlElement("Ende")]
		public string Ende { get; set; }

		/// <summary> Gibt den Kostenträger des Versicherten an. Es handelt sich um das bundesweit gültige Institutions-Kennzeichen (IK) des jeweiligen Kostenträgers. </summary>
		[XmlElement("Kostentraeger")]
		public Kostentraeger Kostentraeger { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true)]
	public class Kostentraeger
	{
		/// <summary> Gibt den Kostenträger des Versicherten an. Es handelt sich um das bundesweit gültige Institutions-Kennzeichen (IK) des jeweiligen Kostenträgers. </summary>
		[XmlElement("Kostentraegerkennung",DataType = "integer")]
		public string Kostentraegerkennung { get; set; }

		/// <summary> Gibt den Kostenträgerländercode vom Kostenträger des Versicherten an </summary>
		[XmlElement("Kostentraegerlaendercode")]
		public string Kostentraegerlaendercode { get; set; }

		/// <summary> Gibt den Namen der Institution bzw. Organisation an. </summary>
		[XmlElement("Name")]
		public string Name { get; set; }

		/// <summary> Identifiziert den abrechnenden Kostenträger. Für diesen sind die Kostenträgerkennung (IK) und der Name des Kostenträgers anzugeben. </summary>
		[XmlElement("AbrechnenderKostentraeger")]
		public Kostentraeger AbrechnenderKostentraeger { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true)]
	public class Zusatzinfos
	{
		/// <summary> Diese Datenobjekte werden ausschließlich für GKV-Versicherte realisiert. </summary>
		[XmlElement("ZusatzinfosGKV")]
		public ZusatzinfosGKV ZusatzinfosGKV { get; set; }

		/// <summary> Diese Datenobjekte werden ausschließlich für PKV-Versicherte realisiert. (nur v5.1) </summary>
		[Obsolete("nur v5.1")]
		[XmlElement("ZusatzinfosPKV")]
		public ZusatzinfosPKV ZusatzinfosPKV { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true)]
	public class ZusatzinfosGKV
	{
		/// <summary> Hier wird der gültige Rechtskreis gemäß Schlüsseltabelle angegeben (SGB V). (nur v5.1) </summary>
		[Obsolete("nur v5.1")]
		[XmlElement("Rechtskreis")]
		public string Rechtskreis { get; set; }

		/// <summary> Gibt die Versichertenart (Mitglied, Familienversicherter oder Rentner ) des Versicherten gemäß Schlüsseltabelle an. </summary>
		[XmlElement("Versichertenart")]
		public string Versichertenart { get; set; }

		/// <summary> Gibt den Versichertenstatus RSA (Risikostrukturausgleich) des Versicherten gemäß Schlüsseltabelle an. (nur v5.1) </summary>
		[Obsolete("nur v5.1")]
		[XmlElement("Versichertenstatus_RSA")]
		public string Versichertenstatus_RSA { get; set; }

		/// <summary> Gibt an, ob der Versicherte die Kostenerstattung für ambulante Behandlung nach § 13 SGB V gewählt hat. </summary>
		[XmlElement("Zusatzinfos_Abrechnung_GKV")]
		public Zusatzinfos_Abrechnung_GKV Zusatzinfos_Abrechnung_GKV { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true)]
	public class Zusatzinfos_Abrechnung_GKV
	{
		/// <summary> 
		/// Gibt an, ob der Kostenträger den Nachweis der Inanspruchnahme von Leistungen der Abrechnungsart Kostenerstattung auf der eGK speichert. 
		/// vorhanden = Nachweis wird genutzt; 
		/// nicht vorhanden = Nachweis wird nicht genutzt 
		/// (nur v5.2)
		/// </summary>
		[XmlElement("Kostenerstattung")]
		public Kostenerstattung Kostenerstattung { get; set; }


		/// <summary> Gibt an, ob der Versicherte die Kostenerstattung für ambulante Behandlung nach § 13 SGB V gewählt hat. (nur v5.1) </summary>
		[XmlElement("Kostenerstattung_ambulant",DataType = "integer")]
		public string Kostenerstattung_ambulant { get; set; }

		/// <summary>
		/// Gibt an, ob der Versicherte die Kostenerstattung für stationäre Behandlung nach § 13 SGB V gewählt hat.
		/// Die Kostenerstattung stationär kann den Wert = 1 nur annehmen, wenn Kostenerstattung ambulant = 1.
		/// (nur v5.1)
		/// </summary>
		[Obsolete("nur v5.1")]
		[XmlElement("Kostenerstattung_stationaer",DataType = "integer")]
		public string Kostenerstattung_stationaer { get; set; }

		/// <summary> Das Kennzeichen WOP ist gemäß § 2 Abs. 2 der Vereinbarung zur Festsetzung des Durchschnittsbetrages gemäß Artikel 2 § 2 Abs. 2 des Gesetzes zur Einführung des Wohnortprinzips bei Honorarvereinbarungen für Ärzte und Zahnärzte und zur Krankenversichertenkarte gemäß § 291 Abs. 2 Fünftes So-zialgesetzbuch (SGB V) erforderlich </summary>
		[XmlElement("WOP")]
		public string WOP { get; set; }
	}



	/// <summary>  (nur v5.1) </summary>
	[Obsolete("nur v5.1")]
	[Serializable]
	[XmlType(AnonymousType = true)]
	public class ZusatzinfosPKV
	{
		/// <summary>
		/// Der PKV-Verbandstarif wird angegeben durch den Wert:
		/// 01 = Tarif ST (Standardtarif)
		/// 02 = Tarif PSKV (Private studentische Krankenversicherung)
		/// 03 = Basistarif ohne Selbstbeteiligung
		/// 04 = Individualtarif 
		/// 05 = Basistarif mit 300,- € Selbstbeteiligung
		/// 06 = Basistarif mit 600,- € Selbstbeteiligung
		/// 07 = Basistarif mit 900,- € Selbstbeteiligung
		/// 08 = Basistarif mit 1.200,- € Selbstbeteiligung
		/// </summary>
		[XmlElement("PKV_Verbandstarif")]
		public string PKV_Verbandstarif { get; set; }

		/// <summary>
		/// Gibt den Anspruch des Versicherten auf Beihilfe an. Mögliche Werte:
		/// 01 = Beihilfe
		/// 02 = Postbeamtenkrankenkasse (PBeaKK)
		/// 03 = Krankenversorgung der Bundesbahnbeamten (KVB)
		/// </summary>
		[XmlElement("Beihilfeberechtigung")]
		public Beihilfeberechtigung Beihilfeberechtigung { get; set; }

		[XmlElement("StationaereLeistungen")]
		public StationaereLeistungen[] StationaereLeistungen { get; set; }
	}



	/// <summary> (nur v5.2) </summary>
	[Serializable]
	[XmlType(AnonymousType = true)]
	public class Kostenerstattung
	{
		/// <summary> 
		/// Gibt die vom Versicherten gewählte Kostenerstattung für die ärztliche Versorgung an.
		/// 1 (true) = Kostenerstattung für ärztliche Versorgung
		/// 0 (false) = keine Kostenerstattung für ärztliche Versorgung</summary>

		[XmlElement("AerztlicheVersorgung")]
		public bool AerztlicheVersorgung { get; set; }

		/// <summary> 
		/// Gibt die vom Versicherten gewählte Kostenerstattung für zahnärztliche Versorgung an.
		/// 1 (true) = Kostenerstattung für zahnärztliche Versorgung
		/// 0 (false) = keine Kostenerstattung für zahnärztliche Versorgung
		/// </summary>
		[XmlElement("ZahnaerztlicheVersorgung")]
		public bool ZahnaerztlicheVersorgung { get; set; }

		/// <summary> 
		/// Gibt die vom Versicherten gewählte Kostenerstattung für den stationaeren Bereich an.
		/// 1 (true) = Kostenerstattung für stationären Bereich 
		/// 0 (false) = keine Kostenerstattung für stationaeren Bereich
		/// </summary>
		[XmlElement("StationaererBereich")]
		public bool StationaererBereich { get; set; }

		/// <summary> 
		/// Gibt die vom Versicherten gewählte Kostenerstattung für veranlasste Leistungen an.
		/// 1 (true) = Kostenerstattung für veranlasste Leistungen
		/// 0 (false) = keine Kostenerstattung für veranlasste Leistungen
		/// </summary>
		[XmlElement("VeranlassteLeistungen")]
		public bool VeranlassteLeistungen { get; set; }
	}


	/// <summary> (nur v5.1) </summary>
	[Obsolete("nur v5.1")]
	[Serializable]
	[XmlType(AnonymousType = true)]
	public class Beihilfeberechtigung
	{
		/// <summary>
		/// Gibt den Anspruch des Versicherten auf Beihilfe an. Mögliche Werte:
		/// 01 = Beihilfe
		/// 02 = Postbeamtenkrankenkasse (PBeaKK)
		/// 03 = Krankenversorgung der Bundesbahnbeamten (KVB)
		/// </summary>
		[XmlElement("Kennzeichnung")]
		public string Kennzeichnung { get; set; }
	}


	/// <summary> (nur v5.1) </summary>
	[Obsolete("nur v5.1")]
	[Serializable]
	[XmlType(AnonymousType = true)]
	public class StationaereLeistungen
	{
		/// <summary>
		/// Gibt die Art der Unterkunft an, die der Versicherte gewählt hat. 
		/// 
		/// 0  = keine Angabe
		/// 1 = Einbett-Zimmer
		/// 2 = Zweibett-Zimmer
		/// 3 = Mehrbett-Zimmer
		/// 4 = Differenz Zwei- und Einbettzimmerzuschlag
		/// </summary>
		[XmlElement("Stationaere_Wahlleistung_Unterkunft")]
		public string Stationaere_Wahlleistung_Unterkunft { get; set; }

		/// <summary> Gibt den Leistungssatz in Prozent an. Wertebereich 000-100. </summary>
		[XmlElement("Prozentwert_Wahlleistung_Unterkunft")]
		public decimal Prozentwert_Wahlleistung_Unterkunft { get; set; }

		/// <summary> Gibt den Höchstbetrag der Erstattung für die Wahlleistung Unterkunft an. </summary>
		[XmlElement("HoechstsatzWahlleistungUnterkunft")]
		public decimal HoechstsatzWahlleistungUnterkunft { get; set; }

		/// <summary>
		/// Gibt den Umfang der ärztlichen Behandlung im stationären Bereich an.
		/// 01 = Gesondert berechenbare Leistungen (Chefarztbehandlung)
		/// 02 = Allgemeine ärztliche Krankenhausleistung
		/// </summary>
		[XmlElement("Stationaere_Wahlleistung_aerztliche_Behandlung",DataType = "integer")]
		public string Stationaere_Wahlleistung_aerztliche_Behandlung { get; set; }

		/// <summary> Gibt den Leistungssatz in Prozent an. Wertebereich 000-100. </summary>
		[XmlElement("Prozentwert_Wahlleistung_aerztliche_Behandlung")]
		public decimal Prozentwert_Wahlleistung_aerztliche_Behandlung { get; set; }

		/// <summary> Gibt die Teilnahme des Kostenträgers des Versicherten am ClinicCard-Verfahren an. </summary>
		[XmlElement("Teilnahme_ClinicCard_Verfahren")]
		public string Teilnahme_ClinicCard_Verfahren { get; set; }
	}
}