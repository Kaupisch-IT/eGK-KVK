namespace CardReader.Egk.PersoenlicheVersichertendaten
{
	public interface IPersoenlicheVersichertendaten
	{
		/// <summary> Die Versicherten-ID ist der 10-stellige unveränderliche Teil der 30-stelligen Krankenversichertennummer. </summary>
		IVersicherter Versicherter { get; }

		string CDM_VERSION { get; }
	}


	public interface IVersicherter
	{
		/// <summary> Die Versicherten-ID ist der 10-stellige unveränderliche Teil der 30-stelligen Krankenversichertennummer. </summary>
		string Versicherten_ID { get; }
		
		/// <summary> Gibt das Geburtsdatum des Versicherten in dem Format "YYYYMMDD" (ISO-8601)  an. </summary>
		IPerson Person { get; }
	}


	public interface IPerson
	{
		/// <summary> Gibt das Geburtsdatum des Versicherten in dem Format "YYYYMMDD" (ISO-8601)  an. </summary>
		string Geburtsdatum { get; }
		
		/// <summary>
		/// Alle Vornamen der Person (max. 5) werden eingegeben. Mehrere Vornamen werden durch Leerzeichen oder Bindestrich getrennt.
		/// Feldlänge 2-45
		/// </summary>
		string Vorname { get; }
		
		/// <summary> Gibt den Nachnamen der Person an. </summary>
		string Nachname { get; }
		
		/// <summary>
		/// Gibt das Geschlecht des Versicherten an gemäss entsprechender Schlüsseltabelle.
		/// "M" = männlich, "W" = weiblich
		/// </summary>
		string Geschlecht { get; }
		
		/// <summary> Gibt die Vorsatzwörter der Person an. </summary>
		string Vorsatzwort { get; }
		
		/// <summary> Gibt die Namenszusätze der Person an, z.B: Freiherr, gemäß entsprechender Schlüsseltabelle. </summary>
		string Namenszusatz { get; }
		
		/// <summary> Gibt die akademischen Grade der Person an, z.B. "Dr.". </summary>
		string Titel { get; }
		
		/// <summary> Gibt die Postleitzahl der Strassen- oder Postfachadresse an. </summary>
		IPostfachAdresse PostfachAdresse { get; }
		
		/// <summary> Gibt die Postleitzahl der Strassen- oder Postfachadresse an. </summary>
		IStrassenAdresse StrassenAdresse { get; }
	}


	public interface IPostfachAdresse
	{
		/// <summary> Gibt die Postleitzahl der Strassen- oder Postfachadresse an. </summary>
		string Postleitzahl { get; }
		
		/// <summary> Gibt den Ort zur Strassen- oder Postfachadresse an. </summary>
		string Ort { get; }
		
		/// <summary> Gibt das Postfach der Person an. </summary>
		string Postfach { get; }
		
		/// <summary>
		/// Versicherter:
		/// Das Land, in dem der Versicherte seinen Wohnsitz hat gem.  Anlage 8 (Staatsangehörigkeit und Länder-kennzeichen für Auslandsanschriften) V. 2.27 vom 8.11.06 (siehe Fachkonzept VSDM)
		/// Kostenträger:
		/// Der Kostenträgerländercode vom Kostenträger des Versicherten gem.  Anlage 8 (Staatsangehörigkeit und Länderkennzeichen für Auslandsanschriften) V. 2.27 vom 8.11.06 (siehe Fachkonzept VSDM).
		/// </summary>
		ILand Land { get; }
	}


	public interface IStrassenAdresse
	{
		/// <summary> Gibt die Postleitzahl der Strassen- oder Postfachadresse an. </summary>
		string Postleitzahl { get; }
		
		/// <summary> Gibt den Ort zur Strassen- oder Postfachadresse an. </summary>
		string Ort { get; }
		
		/// <summary>
		/// Versicherter:
		/// Das Land, in dem der Versicherte seinen Wohnsitz hat gem.  Anlage 8 (Staatsangehörigkeit und Länder-kennzeichen für Auslandsanschriften) V. 2.27 vom 8.11.06 (siehe Fachkonzept VSDM)
		/// Kostenträger:
		/// Der Kostenträgerländercode vom Kostenträger des Versicherten gem.  Anlage 8 (Staatsangehörigkeit und Länderkennzeichen für Auslandsanschriften) V. 2.27 vom 8.11.06 (siehe Fachkonzept VSDM).
		/// </summary>
		ILand Land { get; }
		
		/// <summary> Gibt den Namen der Strasse der Person an. </summary>
		string Strasse { get; }
		
		/// <summary> Gibt die Hausnummer in der Strasse der Person an. </summary>
		string Hausnummer { get; }
		
		/// <summary> Gibt die relevanten Zusätze zur Anschrift an. </summary>
		string Anschriftenzusatz { get; }
	}


	public interface ILand
	{
		/// <summary> Gibt das Land zu der Strassen- oder Postfachadresse an (siehe Fachkonzept VSDM). </summary>
		string Wohnsitzlaendercode { get; }
	}
}