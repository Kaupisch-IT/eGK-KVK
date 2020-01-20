using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace CardReader.Egk.PersoenlicheVersichertendaten
{
	[Serializable]
	[XmlType(AnonymousType = true)]
	[XmlRoot("UC_PersoenlicheVersichertendatenXML",IsNullable = false)]
	[DebuggerDisplay("{Versicherter.Versicherten_ID} {Versicherter.Person.Vorname} {Versicherter.Person.Nachname}")]
	public class PersoenlicheVersichertendaten
	{
		/// <summary> Die Versicherten-ID ist der 10-stellige unveränderliche Teil der 30-stelligen Krankenversichertennummer. </summary>
		[XmlElement("Versicherter")]
		public Versicherter Versicherter { get; set; }

		[XmlAttribute("CDM_VERSION")]
		public string CDM_VERSION { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true)]
	[DebuggerDisplay("{Versicherten_ID} {Person.Vorname} {Person.Nachname}")]
	public class Versicherter
	{
		/// <summary> Die Versicherten-ID ist der 10-stellige unveränderliche Teil der 30-stelligen Krankenversichertennummer. </summary>
		[XmlElement("Versicherten_ID")]
		public string Versicherten_ID { get; set; }

		/// <summary> Gibt das Geburtsdatum des Versicherten in dem Format "YYYYMMDD" (ISO-8601)  an. </summary>
		[XmlElement("Person")]
		public Person Person { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true)]
	[DebuggerDisplay("{Vorname} {Nachname}")]
	public class Person
	{
		/// <summary> Gibt das Geburtsdatum des Versicherten in dem Format "YYYYMMDD" (ISO-8601)  an. </summary>
		[XmlElement("Geburtsdatum")]
		public string Geburtsdatum { get; set; }

		/// <summary>
		/// Alle Vornamen der Person (max. 5) werden eingegeben. Mehrere Vornamen werden durch Leerzeichen oder Bindestrich getrennt.
		/// Feldlänge 2-45
		/// </summary>
		[XmlElement("Vorname")]
		public string Vorname { get; set; }

		/// <summary> Gibt den Nachnamen der Person an. </summary>
		[XmlElement("Nachname")]
		public string Nachname { get; set; }

		/// <summary>
		/// Gibt das Geschlecht des Versicherten an gemäß entsprechender Schlüsseltabelle.
		/// "M" = männlich, "W" = weiblich
		/// </summary>
		[XmlElement("Geschlecht")]
		public string Geschlecht { get; set; }

		/// <summary> Gibt die Vorsatzwörter der Person an. </summary>
		[XmlElement("Vorsatzwort")]
		public string Vorsatzwort { get; set; }

		/// <summary> Gibt die Namenszusätze der Person an, z.B: Freiherr, gemäß entsprechender Schlüsseltabelle. </summary>
		[XmlElement("Namenszusatz")]
		public string Namenszusatz { get; set; }

		/// <summary> Gibt die akademischen Grade der Person an, z.B. "Dr.". </summary>
		[XmlElement("Titel")]
		public string Titel { get; set; }

		/// <summary> Gibt die Postleitzahl der Straßen- oder Postfachadresse an. </summary>
		[XmlElement("PostfachAdresse")]
		public PostfachAdresse PostfachAdresse { get; set; }

		/// <summary> Gibt die Postleitzahl der Straßen- oder Postfachadresse an. </summary>
		[XmlElement("StrassenAdresse")]
		public StrassenAdresse StrassenAdresse { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true)]
	[DebuggerDisplay("{Postfach} {Land.Wohnsitzlaendercode}-{Postleitzahl} {Ort}")]
	public class PostfachAdresse
	{
		/// <summary> Gibt die Postleitzahl der Straßen- oder Postfachadresse an. </summary>
		[XmlElement("Postleitzahl")]
		public string Postleitzahl { get; set; }

		/// <summary> Gibt den Ort zur Straßen- oder Postfachadresse an. </summary>
		[XmlElement("Ort")]
		public string Ort { get; set; }

		/// <summary> Gibt das Postfach der Person an. </summary>
		[XmlElement("Postfach")]
		public string Postfach { get; set; }

		/// <summary>
		/// Versicherter: Das Land, in dem der Versicherte seinen Wohnsitz hat gem.  Anlage 8 (Staatsangehörigkeit und Länderkennzeichen für Auslandsanschriften) V. 2.27 vom 8.11.06 (siehe Fachkonzept VSDM)
		/// Kostenträger: Der Kostenträgerländercode vom Kostenträger des Versicherten gem.  Anlage 8 (Staatsangehörigkeit und Länderkennzeichen für Auslandsanschriften) V. 2.27 vom 8.11.06 (siehe Fachkonzept VSDM).
		/// </summary>
		[XmlElement("Land")]
		public Land Land { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true)]
	[DebuggerDisplay("{Straße} {Hausnummer} {Anschriftenzusatz} {Land.Wohnsitzlaendercode}-{Postleitzahl} {Ort}")]
	public class StrassenAdresse
	{
		/// <summary> Gibt die Postleitzahl der Straßen- oder Postfachadresse an. </summary>
		[XmlElement("Postleitzahl")]
		public string Postleitzahl { get; set; }

		/// <summary> Gibt den Ort zur Straßen- oder Postfachadresse an. </summary>
		[XmlElement("Ort")]
		public string Ort { get; set; }

		/// <summary>
		/// Versicherter: Das Land, in dem der Versicherte seinen Wohnsitz hat gem.  Anlage 8 (Staatsangehörigkeit und Länderkennzeichen für Auslandsanschriften) V. 2.27 vom 8.11.06 (siehe Fachkonzept VSDM)
		/// Kostenträger: Der Kostenträgerländercode vom Kostenträger des Versicherten gem.  Anlage 8 (Staatsangehörigkeit und Länderkennzeichen für Auslandsanschriften) V. 2.27 vom 8.11.06 (siehe Fachkonzept VSDM).
		/// </summary>
		[XmlElement("Land")]
		public Land Land { get; set; }

		/// <summary> Gibt den Namen der Straße der Person an. </summary>
		[XmlElement("Straße")]
		public string Strasse { get; set; }

		/// <summary> Gibt die Hausnummer in der Straße der Person an. </summary>
		[XmlElement("Hausnummer")]
		public string Hausnummer { get; set; }

		/// <summary> Gibt die relevanten Zusätze zur Anschrift an. </summary>
		[XmlElement("Anschriftenzusatz")]
		public string Anschriftenzusatz { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true)]
	[DebuggerDisplay("{Wohnsitzlaendercode}")]
	public class Land
	{
		/// <summary> Gibt das Land zu der Straßen- oder Postfachadresse an (siehe Fachkonzept VSDM). </summary>
		[XmlElement("Wohnsitzlaendercode")]
		public string Wohnsitzlaendercode { get; set; }
	}
}