namespace CardReader.Egk.PersoenlicheVersichertendaten
{
	public interface IPersoenlicheVersichertendaten
	{
		IVersicherter Versicherter { get; }
		string CDM_VERSION { get; }
	}


	public interface IVersicherter
	{
		string Versicherten_ID { get; }
		IPerson Person { get; }
	}


	public interface IPerson
	{
		string Geburtsdatum { get; }
		string Vorname { get; }
		string Nachname { get; }
		string Geschlecht { get; }
		string Vorsatzwort { get; }
		string Namenszusatz { get; }
		string Titel { get; }
		IPostfachAdresse PostfachAdresse { get; }
		IStrassenAdresse StrassenAdresse { get; }
	}


	public interface IPostfachAdresse
	{
		string Postleitzahl { get; }
		string Ort { get; }
		string Postfach { get; }
		ILand Land { get; }
	}


	public interface IStrassenAdresse
	{
		string Postleitzahl { get; }
		string Ort { get; }
		ILand Land { get; }
		string Strasse { get; }
		string Hausnummer { get; }
		string Anschriftenzusatz { get; }
	}


	public interface ILand
	{
		string Wohnsitzlaendercode { get; }
	}
}