using System;
using System.Collections.Generic;
using System.Linq;

namespace KaupischIT.CardReader
{
	public class CardResult
	{
		/// <summary> 
		/// Ruft die ausgelesenen Daten einer elektronischen Gesundheitskarte (eGK) ab. 
		/// </summary>
		public EgkResult EgkResult { get; set; }

		/// <summary> 
		/// Ruft die ausgelesenen Daten einer Card für Privatversicherte (PKV-Card) ab. 
		/// </summary>
		public PkvResult PkvResult { get; set; }


		/// <summary> Ruft ab, ob der Auslesevorgang für eine eGK oder eine KVK/PKV-Card erfolgreich war </summary>
		public bool Success
			=> this.EgkResult!=null || this.PkvResult!=null;


		/// <summary> Gibt die Bezeichnung und Ort der ausstellenden Krankenkasse/des ausstellenden Kostenträgers an. </summary>
		public string KostentraegerName
			=> this.PkvResult?.KrankenKassenName
			?? this.EgkResult?.AllgemeineVersicherungsdaten?.Versicherter?.Versicherungsschutz?.Kostentraeger?.Name;

		/// <summary> Gibt die Krankenkasse/den Kostenträger des Versicherten an. Es handelt sich um das bundesweit gültige Institutions-Kennzeichen (IK) des jeweiligen Kostenträgers. </summary>
		public string Kostentraegerkennung
			=> this.EgkResult?.AllgemeineVersicherungsdaten?.Versicherter?.Versicherungsschutz?.Kostentraeger?.Kostentraegerkennung;

		/// <summary> Die Versicherten-ID ist der 10-stellige unveränderliche Teil der 30-stelligen Krankenversichertennummer. </summary>
		public string VersichertenID
			=> this.PkvResult?.VersicherungsNummer 
			?? this.EgkResult?.PersoenlicheVersichertendaten?.Versicherter?.Versicherten_ID;

		/// <summary> Gibt die Versichertenart (Mitglied, Familienversicherter oder Rentner ) des Versicherten gemäß Schlüsseltabelle an. </summary>
		public string Versichertenart
			=> this.EgkResult?.AllgemeineVersicherungsdaten?.Versicherter?.Zusatzinfos.ZusatzinfosGKV?.Versichertenart;

		/// <summary> Gibt die akademischen Grade der Person an, z.B. "Dr.". </summary>
		public string Titel
			=> this.PkvResult?.Titel 
			?? this.EgkResult?.PersoenlicheVersichertendaten?.Versicherter?.Person?.Titel;

		/// <summary>
		/// Alle Vornamen der Person (max. 5) werden eingegeben. Mehrere Vornamen werden durch Leerzeichen oder Bindestrich getrennt.
		/// </summary>
		public string Vorname
			=> this.PkvResult?.VorName 
			?? this.EgkResult?.PersoenlicheVersichertendaten?.Versicherter?.Person?.Vorname;

		/// <summary> Gibt den Nachnamen der Person an. </summary>
		public string Nachname
			=> this.PkvResult?.FamilienName 
			?? this.EgkResult?.PersoenlicheVersichertendaten?.Versicherter?.Person?.Nachname;

		/// <summary>
		/// Gibt das Geschlecht des Versicherten an gemäß entsprechender Schlüsseltabelle.
		/// "M" = männlich, "W" = weiblich
		/// </summary>
		public string Geschlecht
			=> this.EgkResult?.PersoenlicheVersichertendaten?.Versicherter?.Person?.Geschlecht;

		/// <summary> Gibt die Namenszusätze der Person an, z.B: Freiherr, gemäß entsprechender Schlüsseltabelle. </summary>
		public string Namenszusatz_Vorsatzwort
			=> this.PkvResult?.NamensZusatz_VorsatzWort 
			?? Join(this.EgkResult?.PersoenlicheVersichertendaten?.Versicherter?.Person?.Namenszusatz,this.EgkResult?.PersoenlicheVersichertendaten?.Versicherter?.Person?.Vorsatzwort);

		/// <summary> Gibt das Geburtsdatum des Versicherten in dem Format "YYYYMMDD" (ISO-8601)  an. </summary>
		public string Geburtsdatum
			=> this.PkvResult?.GeburtsDatum 
			?? this.EgkResult?.PersoenlicheVersichertendaten?.Versicherter?.Person?.Geburtsdatum;

		/// <summary> Gibt den Namen der Straße der Person an. </summary>
		public string Strasse_Hausnummer
			=> this.PkvResult?.StraßenName_HausNummer 
			?? Join(this.EgkResult?.PersoenlicheVersichertendaten?.Versicherter?.Person?.StrassenAdresse?.Strasse,this.EgkResult?.PersoenlicheVersichertendaten?.Versicherter?.Person?.StrassenAdresse?.Hausnummer);

		/// <summary> Gibt das Land zu der Straßen- oder Postfachadresse an (wenn nicht vorhanden: Deutschland). </summary>
		public string Wohnsitzlaendercode
			=> this.PkvResult?.WohnsitzLänderCode
			?? this.EgkResult?.PersoenlicheVersichertendaten?.Versicherter?.Person?.StrassenAdresse?.Land?.Wohnsitzlaendercode;

		/// <summary> Gibt die Postleitzahl der Straßen- oder Postfachadresse an. </summary>
		public string Postleitzahl
			=> this.PkvResult?.Postleitzahl 
			?? this.EgkResult?.PersoenlicheVersichertendaten?.Versicherter?.Person?.StrassenAdresse?.Postleitzahl;

		/// <summary> Gibt den Ort zur Straßen- oder Postfachadresse an. </summary>
		public string Ort
			=> this.PkvResult?.OrtsName 
			?? this.EgkResult?.PersoenlicheVersichertendaten?.Versicherter?.Person?.StrassenAdresse?.Ort;

		/// <summary> Gibt bei befristeter Gültigkeit der Karte den Monat des Fristablaufs an. Üblich bei ausreichend langer Mitgliedschaft sind Gültigkeitsdauern von fünf bis zehn Jahren oder auch mehr. </summary>	
		public string VersicherungsschutzEnde
			=> this.PkvResult?.GültigkeitsDatum
			?? this.EgkResult?.AllgemeineVersicherungsdaten?.Versicherter?.Versicherungsschutz?.Ende;


		private static string Join(params string[] values)
		{
			IEnumerable<string> nonEmptyValues = values.Where(v => !String.IsNullOrEmpty(v));
			return (nonEmptyValues.Any()) ? String.Join(" ",nonEmptyValues) : null;
		}
	}
}
