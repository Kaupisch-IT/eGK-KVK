using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace CardReader.Egk.PersoenlicheVersichertendaten
{
	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_PersoenlicheVersichertendatenXML/v5.1")]
	[XmlRoot("UC_PersoenlicheVersichertendatenXML",Namespace = "http://ws.gematik.de/fa/vsds/UC_PersoenlicheVersichertendatenXML/v5.1",IsNullable = false)]
	[DebuggerDisplay("{Versicherter.Versicherten_ID} {Versicherter.Person.Vorname} {Versicherter.Person.Nachname}")]
	public class PersoenlicheVersichertendaten51 : IPersoenlicheVersichertendaten
	{
		/// <summary> Die Versicherten-ID ist der 10-stellige unveränderliche Teil der 30-stelligen Krankenversichertennummer. </summary>
		[XmlElement("Versicherter")]
		public Versicherter51 Versicherter { get; set; }

		[XmlAttribute("CDM_VERSION")]
		public string CDM_VERSION { get; set; }


		IVersicherter IPersoenlicheVersichertendaten.Versicherter { get { return this.Versicherter; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_PersoenlicheVersichertendatenXML/v5.1")]
	[DebuggerDisplay("{Versicherten_ID} {Person.Vorname} {Person.Nachname}")]
	public class Versicherter51 : IVersicherter
	{
		/// <summary> Die Versicherten-ID ist der 10-stellige unveränderliche Teil der 30-stelligen Krankenversichertennummer. </summary>
		[XmlElement("Versicherten_ID")]
		public string Versicherten_ID { get; set; }

		/// <summary> Gibt das Geburtsdatum des Versicherten in dem Format "YYYYMMDD" (ISO-8601)  an. </summary>
		[XmlElement("Person")]
		public Person51 Person { get; set; }


		IPerson IVersicherter.Person { get { return this.Person; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_PersoenlicheVersichertendatenXML/v5.1")]
	[DebuggerDisplay("{Vorname} {Nachname}")]
	public class Person51 : IPerson
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
		/// Gibt das Geschlecht des Versicherten an gemäss entsprechender Schlüsseltabelle.
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

		/// <summary> Gibt die Postleitzahl der Strassen- oder Postfachadresse an. </summary>
		[XmlElement("PostfachAdresse")]
		public PostfachAdresse51 PostfachAdresse { get; set; }

		/// <summary> Gibt die Postleitzahl der Strassen- oder Postfachadresse an. </summary>
		[XmlElement("StrassenAdresse")]
		public StrassenAdresse51 StrassenAdresse { get; set; }


		IPostfachAdresse IPerson.PostfachAdresse { get { return this.PostfachAdresse; } }
		IStrassenAdresse IPerson.StrassenAdresse { get { return this.StrassenAdresse; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_PersoenlicheVersichertendatenXML/v5.1")]
	[DebuggerDisplay("{Postfach} {Land.Wohnsitzlaendercode}-{Postleitzahl} {Ort}")]
	public class PostfachAdresse51 : IPostfachAdresse
	{
		/// <summary> Gibt die Postleitzahl der Strassen- oder Postfachadresse an. </summary>
		[XmlElement("Postleitzahl")]
		public string Postleitzahl { get; set; }

		/// <summary> Gibt den Ort zur Strassen- oder Postfachadresse an. </summary>
		[XmlElement("Ort")]
		public string Ort { get; set; }

		/// <summary> Gibt das Postfach der Person an. </summary>
		[XmlElement("Postfach")]
		public string Postfach { get; set; }

		/// <summary>
		/// Versicherter:
		/// Das Land, in dem der Versicherte seinen Wohnsitz hat gem.  Anlage 8 (Staatsangehörigkeit und Länder-kennzeichen für Auslandsanschriften) V. 2.27 vom 8.11.06 (siehe Fachkonzept VSDM)
		/// Kostenträger:
		/// Der Kostenträgerländercode vom Kostenträger des Versicherten gem.  Anlage 8 (Staatsangehörigkeit und Länderkennzeichen für Auslandsanschriften) V. 2.27 vom 8.11.06 (siehe Fachkonzept VSDM).
		/// </summary>
		[XmlElement("Land")]
		public Land51 Land { get; set; }


		ILand IPostfachAdresse.Land { get { return this.Land; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_PersoenlicheVersichertendatenXML/v5.1")]
	[DebuggerDisplay("{Strasse} {Hausnummer} {Anschriftenzusatz} {Land.Wohnsitzlaendercode}-{Postleitzahl} {Ort}")]
	public class StrassenAdresse51 : IStrassenAdresse
	{
		/// <summary> Gibt die Postleitzahl der Strassen- oder Postfachadresse an. </summary>
		[XmlElement("Postleitzahl")]
		public string Postleitzahl { get; set; }

		/// <summary> Gibt den Ort zur Strassen- oder Postfachadresse an. </summary>
		[XmlElement("Ort")]
		public string Ort { get; set; }

		/// <summary>
		/// Versicherter:
		/// Das Land, in dem der Versicherte seinen Wohnsitz hat gem.  Anlage 8 (Staatsangehörigkeit und Länder-kennzeichen für Auslandsanschriften) V. 2.27 vom 8.11.06 (siehe Fachkonzept VSDM)
		/// Kostenträger:
		/// Der Kostenträgerländercode vom Kostenträger des Versicherten gem.  Anlage 8 (Staatsangehörigkeit und Länderkennzeichen für Auslandsanschriften) V. 2.27 vom 8.11.06 (siehe Fachkonzept VSDM).
		/// </summary>
		[XmlElement("Land")]
		public Land51 Land { get; set; }

		/// <summary> Gibt den Namen der Strasse der Person an. </summary>
		[XmlElement("Strasse")]
		public string Strasse { get; set; }

		/// <summary> Gibt die Hausnummer in der Strasse der Person an. </summary>
		[XmlElement("Hausnummer")]
		public string Hausnummer { get; set; }

		/// <summary> Gibt die relevanten Zusätze zur Anschrift an. </summary>
		[XmlElement("Anschriftenzusatz")]
		public string Anschriftenzusatz { get; set; }


		ILand IStrassenAdresse.Land { get { return this.Land; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsds/UC_PersoenlicheVersichertendatenXML/v5.1")]
	[DebuggerDisplay("{Wohnsitzlaendercode}")]
	public class Land51 : ILand
	{
		/// <summary> Gibt das Land zu der Strassen- oder Postfachadresse an (siehe Fachkonzept VSDM). </summary>
		[XmlElement("Wohnsitzlaendercode")]
		public string Wohnsitzlaendercode { get; set; }
	}
}
