using System;
using System.Xml.Serialization;
using CardReader.Egk.AllgemeineVersicherungsdaten;

namespace CardReader.Egk.AllgemeineVersicherungsdaten
{
	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	[XmlRoot("UC_AllgemeineVersicherungsdatenXML",Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2",IsNullable = false)]
	public class AllgemeineVersicherungsdaten52 : IAllgemeineVersicherungsdaten
	{
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
		[XmlElement("Versicherungsschutz")]
		public Versicherungsschutz52 Versicherungsschutz { get; set; }

		[XmlElement("Zusatzinfos")]
		public Zusatzinfos52 Zusatzinfos { get; set; }


		IVersicherungsschutz IVersicherter.Versicherungsschutz { get { return this.Versicherungsschutz; } }
		IZusatzinfos IVersicherter.Zusatzinfos { get { return this.Zusatzinfos; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class Versicherungsschutz52 : IVersicherungsschutz
	{
		[XmlElement("Beginn")]
		public string Beginn { get; set; }

		[XmlElement("Ende")]
		public string Ende { get; set; }

		[XmlElement("Kostentraeger")]
		public Kostentraeger52 Kostentraeger { get; set; }

		IKostentraeger IVersicherungsschutz.Kostentraeger { get { return this.Kostentraeger; } }
	}



	[Serializable]
	[XmlType(Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class Kostentraeger52 : IKostentraeger
	{
		[XmlElement("Kostentraegerkennung",DataType = "integer")]
		public string Kostentraegerkennung { get; set; }

		[XmlElement("Kostentraegerlaendercode")]
		public string Kostentraegerlaendercode { get; set; }

		[XmlElement("Name")]
		public string Name { get; set; }

		[XmlElement("AbrechnenderKostentraeger")]
		public Kostentraeger52 AbrechnenderKostentraeger { get; set; }


		IKostentraeger IKostentraeger.AbrechnenderKostentraeger { get { return this.AbrechnenderKostentraeger; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class Zusatzinfos52 : IZusatzinfos
	{
		[XmlElement("ZusatzinfosGKV")]
		public ZusatzinfosGKV52 ZusatzinfosGKV { get; set; }


		object IZusatzinfos.Item { get { return this.ZusatzinfosGKV; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class ZusatzinfosGKV52 : IZusatzinfosGKV
	{
		[XmlElement("Versichertenart")]
		public string Versichertenart { get; set; }

		[XmlElement("Zusatzinfos_Abrechnung_GKV")]
		public Zusatzinfos_Abrechnung_GKV52 Zusatzinfos_Abrechnung_GKV { get; set; }


		IZusatzinfos_Abrechnung_GKV IZusatzinfosGKV.Zusatzinfos_Abrechnung_GKV { get { return this.Zusatzinfos_Abrechnung_GKV; } }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class Zusatzinfos_Abrechnung_GKV52 : IZusatzinfos_Abrechnung_GKV
	{
		[XmlElement("Kostenerstattung")]
		public Kostenerstattung52 Kostenerstattung { get; set; }

		[XmlElement("WOP")]
		public string WOP { get; set; }
	}



	[Serializable]
	[XmlType(AnonymousType = true,Namespace = "http://ws.gematik.de/fa/vsdm/vsd/v5.2")]
	public class Kostenerstattung52
	{
		[XmlElement("AerztlicheVersorgung")]
		public bool AerztlicheVersorgung { get; set; }

		[XmlElement("ZahnaerztlicheVersorgung")]
		public bool ZahnaerztlicheVersorgung { get; set; }

		[XmlElement("StationaererBereich")]
		public bool StationaererBereich { get; set; }

		[XmlElement("VeranlassteLeistungen")]
		public bool VeranlassteLeistungen { get; set; }
	}
}