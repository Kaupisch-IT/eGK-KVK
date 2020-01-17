using System;
using System.Xml.Serialization;

namespace CardReader.Egk.AllgemeineVersicherungsdaten
{
	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	[XmlRoot("UC_AllgemeineVersicherungsdatenXML",Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2",IsNullable = false)]
	public class AllgemeineVersicherungsdaten52 : IAllgemeineVersicherungsdaten
	{
		/// <summary> Die Versicherten-ID ist der 10-stellige unveränderliche Teil der 30-stelligen Krankenversichertennummer. </summary>
		[XmlElement("Versicherter")]
		public Versicherter52 Versicherter { get; set; }

		[XmlAttribute("CDM_VERSION")]
		public string CDM_VERSION { get; set; }


		IVersicherter IAllgemeineVersicherungsdaten.Versicherter { get { return this.Versicherter; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class Versicherter52 : IVersicherter
	{
		/// <summary>
		/// Gibt den Beginn des Versicherungsschutzes (hier: Leistungsanspruch) des Versicherten bei dem unter Klasse Kostenträger angegebenen Kostenträger an. 
		/// Format: YYYYMMDD (ISO-8601)
		/// </summary>
		[XmlElement("Versicherungsschutz")]
		public Versicherungsschutz52 Versicherungsschutz { get; set; }

		/// <summary>
		/// Das Informationsmodell VSD beinhaltet Daten für GKV und PKV.
		/// Für die Abbildung des definitiven Datensatzes der VSD wird aber nur die jeweils zutreffende Klasse GKV oder PKV realisiert, d.h. es erfolgte keine Abbildung von PKV-Informationen in den VSD der GKV und umgekehrt.
		/// </summary>
		[XmlElement("Zusatzinfos")]
		public Zusatzinfos52 Zusatzinfos { get; set; }


		IVersicherungsschutz IVersicherter.Versicherungsschutz { get { return this.Versicherungsschutz; } }
		IZusatzinfos IVersicherter.Zusatzinfos { get { return this.Zusatzinfos; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class Versicherungsschutz52 : IVersicherungsschutz
	{
		/// <summary>
		/// Gibt den Beginn des Versicherungsschutzes (hier: Leistungsanspruch) des Versicherten bei dem unter Klasse Kostenträger angegebenen Kostenträger an. 
		/// Format: YYYYMMDD (ISO-8601)
		/// </summary>

		[XmlElement("Beginn")]
		public string Beginn { get; set; }

		/// <summary>
		/// Gibt das Ende des Versicherungsschutzes (hier: Leistungsanspruch) des Ver-sicherten bei dem unter Klasse Kostenträger angegebenen Kostenträger an. 
		/// Format: YYYYMMDD (ISO-8601)
		/// </summary>
		[XmlElement("Ende")]
		public string Ende { get; set; }

		/// <summary> Gibt den Kostenträger des Versicherten an. Es handelt sich um das bundesweit gültige Institutionskennzeichen (IK) des jeweiligen Kostenträgers. </summary>
		[XmlElement("Kostentraeger")]
		public Kostentraeger52 Kostentraeger { get; set; }


		IKostentraeger IVersicherungsschutz.Kostentraeger { get { return this.Kostentraeger; } }
	}



	[Serializable]
	[XmlType(Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class Kostentraeger52 : IKostentraeger
	{
		/// <summary> Gibt den Kostenträger des Versicherten an. Es handelt sich um das bundesweit gültige Institutionskennzeichen (IK) des jeweiligen Kostenträgers. </summary>
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
		public Kostentraeger52 AbrechnenderKostentraeger { get; set; }


		IKostentraeger IKostentraeger.AbrechnenderKostentraeger { get { return this.AbrechnenderKostentraeger; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class Zusatzinfos52 : IZusatzinfos
	{
		/// <summary> Diese Datenobjekte werden ausschließlich für GKV-Versicherte realisiert. </summary>
		[XmlElement("ZusatzinfosGKV")]
		public ZusatzinfosGKV52 ZusatzinfosGKV { get; set; }


		object IZusatzinfos.Item { get { return this.ZusatzinfosGKV; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class ZusatzinfosGKV52 : IZusatzinfosGKV
	{
		/// <summary> Gibt die Versichertenart (Mitglied, Familienversicherter oder Rentner ) des Versicherten gemäß Schlüsseltabelle an. </summary>
		[XmlElement("Versichertenart")]
		public string Versichertenart { get; set; }

		/// <summary> Gibt an, ob der Versicherte die Kostenerstattung für ambulante Behandlung nach § 13 SGB V gewählt hat. </summary>
		[XmlElement("Zusatzinfos_Abrechnung_GKV")]
		public Zusatzinfos_Abrechnung_GKV52 Zusatzinfos_Abrechnung_GKV { get; set; }


		IZusatzinfos_Abrechnung_GKV IZusatzinfosGKV.Zusatzinfos_Abrechnung_GKV { get { return this.Zusatzinfos_Abrechnung_GKV; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class Zusatzinfos_Abrechnung_GKV52 : IZusatzinfos_Abrechnung_GKV
	{
		/// <summary> 
		/// Gibt an, ob der Kostentraeger den Nachweis der Inanspruchnahme von Leistungen der Abrechnungsart Kostenerstattung auf der eGK speichert. 
		/// vorhanden = Nachweis wird genutzt; 
		/// nichtvorhanden = Nachweis wird nicht genutzt 
		/// </summary>
		[XmlElement("Kostenerstattung")]
		public Kostenerstattung52 Kostenerstattung { get; set; }

		/// <summary> Das Kennzeichen WOP ist gemäß § 2 Abs. 2 der Vereinbarung zur Festset-zung des Durchschnittsbetrages gemäß Artikel 2 § 2 Abs. 2 des Gesetzes zur Einführung des Wohnortprinzips bei Honorarvereinbarungen für Ärzte und Zahnärzte und zur Krankenversichertenkarte gemäß § 291 Abs. 2 Fünftes So-zialgesetzbuch (SGB V) erforderlich </summary>
		[XmlElement("WOP")]
		public string WOP { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class Kostenerstattung52
	{
		/// <summary> 
		/// Gibt die vom Versicherten gewaehlte Kostenerstattung fuer die aerztliche Versorgung an.
		/// 1 (true) = Kostenerstattung fuer aerztliche Versorgung
		/// 0 (false) = keine Kostenerstattung fuer aerztliche Versorgung</summary>

		[XmlElement("AerztlicheVersorgung")]
		public bool AerztlicheVersorgung { get; set; }

		/// <summary> 
		/// Gibt die vom Versicherten gewaehlte Kostenerstattung fuer zahnaerztliche Versorgung an.
		/// 1 (true) = Kostenerstattung fuer zahnaerztliche Versorgung
		/// 0 (false) = keine Kostenerstattung fuer zahnaerztliche Versorgung
		/// </summary>
		[XmlElement("ZahnaerztlicheVersorgung")]
		public bool ZahnaerztlicheVersorgung { get; set; }

		/// <summary> 
		/// Gibt die vom Versicherten gewaehlte Kostenerstattung fuer den stationaeren Bereich an.
		/// 1 (true) = Kostenerstattung fuer stationaeren Bereich 
		/// 0 (false) = keine Kostenerstattung fuer stationaeren Bereich
		/// </summary>
		[XmlElement("StationaererBereich")]
		public bool StationaererBereich { get; set; }

		/// <summary> 
		/// Gibt die vom Versicherten gewaehlte Kostenerstattung fuer veranlasste Leistungen an.
		/// 1 (true) = Kostenerstattung fuer veranlasste Leistungen
		/// 0 (false) = keine Kostenerstattung fuer veranlasste Leistungen
		/// </summary>
		[XmlElement("VeranlassteLeistungen")]
		public bool VeranlassteLeistungen { get; set; }
	}
}