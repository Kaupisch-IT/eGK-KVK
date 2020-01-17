using System;
using System.Diagnostics;
using System.Xml.Serialization;
using CardReader.Egk.PersoenlicheVersichertendaten;

namespace CardReader.Egk.PersoenlicheVersichertendaten
{
	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	[XmlRoot("UC_PersoenlicheVersichertendatenXML",Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2",IsNullable = false)]
	[DebuggerDisplay("{Versicherter.Versicherten_ID} {Versicherter.Person.Vorname} {Versicherter.Person.Nachname}")]
	public class PersoenlicheVersichertendaten52 : IPersoenlicheVersichertendaten
	{
		[XmlElement("Versicherter")]
		public Versicherter52 Versicherter { get; set; }

		[XmlAttribute("CDM_VERSION")]
		public string CDM_VERSION { get; set; }


		IVersicherter IPersoenlicheVersichertendaten.Versicherter { get { return this.Versicherter; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	[DebuggerDisplay("{Versicherten_ID} {Person.Vorname} {Person.Nachname}")]
	public class Versicherter52 : IVersicherter
	{
		[XmlElement("Versicherten_ID")]
		public string Versicherten_ID { get; set; }

		[XmlElement("Person")]
		public Person52 Person { get; set; }


		IPerson IVersicherter.Person { get { return this.Person; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	[DebuggerDisplay("{Vorname} {Nachname}")]
	public class Person52 : IPerson
	{
		[XmlElement("Geburtsdatum")]
		public string Geburtsdatum { get; set; }

		[XmlElement("Vorname")]
		public string Vorname { get; set; }

		[XmlElement("Nachname")]
		public string Nachname { get; set; }

		[XmlElement("Geschlecht")]
		public string Geschlecht { get; set; }

		[XmlElement("Vorsatzwort")]
		public string Vorsatzwort { get; set; }

		[XmlElement("Namenszusatz")]
		public string Namenszusatz { get; set; }

		[XmlElement("Titel")]
		public string Titel { get; set; }

		[XmlElement("PostfachAdresse")]
		public PostfachAdresse52 PostfachAdresse { get; set; }

		[XmlElement("StrassenAdresse")]
		public StrassenAdresse52 StrassenAdresse { get; set; }


		IPostfachAdresse IPerson.PostfachAdresse { get { return this.PostfachAdresse; } }
		IStrassenAdresse IPerson.StrassenAdresse { get { return this.StrassenAdresse; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	[DebuggerDisplay("{Postfach} {Land.Wohnsitzlaendercode}-{Postleitzahl} {Ort}")]
	public class PostfachAdresse52 : IPostfachAdresse
	{
		[XmlElement("Postleitzahl")]
		public string Postleitzahl { get; set; }

		[XmlElement("Ort")]
		public string Ort { get; set; }

		[XmlElement("Postfach")]
		public string Postfach { get; set; }

		[XmlElement("Land")]
		public Land52 Land { get; set; }


		ILand IPostfachAdresse.Land { get { return this.Land; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	[DebuggerDisplay("{Strasse} {Hausnummer} {Anschriftenzusatz} {Land.Wohnsitzlaendercode}-{Postleitzahl} {Ort}")]
	public class StrassenAdresse52 : IStrassenAdresse
	{
		[XmlElement("Postleitzahl")]
		public string Postleitzahl { get; set; }

		[XmlElement("Ort")]
		public string Ort { get; set; }

		[XmlElement("Land")]
		public Land52 Land { get; set; }

		[XmlElement("Strasse")]
		public string Strasse { get; set; }

		[XmlElement("Hausnummer")]
		public string Hausnummer { get; set; }

		[XmlElement("Anschriftenzusatz")]
		public string Anschriftenzusatz { get; set; }


		ILand IStrassenAdresse.Land { get { return this.Land; } }
	}



	[Serializable]
	[XmlType(Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	[DebuggerDisplay("{Wohnsitzlaendercode}")]
	public class Land52 : ILand
	{
		[XmlElement("Wohnsitzlaendercode")]
		public string Wohnsitzlaendercode { get; set; }
	}
}