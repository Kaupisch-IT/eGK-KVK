namespace CardReader.Egk.AllgemeineVersicherungsdaten
{
	public interface IAllgemeineVersicherungsdaten
	{
		IVersicherter Versicherter { get; }
		string CDM_VERSION { get; }
	}


	public interface IVersicherter
	{
		IVersicherungsschutz Versicherungsschutz { get; }
		IZusatzinfos Zusatzinfos { get; }
	}


	public interface IVersicherungsschutz
	{
		string Beginn { get; }
		string Ende { get; }
		IKostentraeger Kostentraeger { get; }
	}


	public interface IKostentraeger
	{
		string Kostentraegerkennung { get; }
		string Kostentraegerlaendercode { get; }
		string Name { get; }
		IKostentraeger AbrechnenderKostentraeger { get; }
	}


	public interface IZusatzinfos
	{
		object Item { get; }
	}


	public interface IZusatzinfosGKV
	{
		string Versichertenart { get; }
		IZusatzinfos_Abrechnung_GKV Zusatzinfos_Abrechnung_GKV { get; }
	}


	public interface IZusatzinfos_Abrechnung_GKV
	{
		string WOP { get; }
	}


	public interface IZusatzinfosPKV
	{
		string PKV_Verbandstarif { get; }

		IBeihilfeberechtigung Beihilfeberechtigung { get; }

		IStationaereLeistungen[] StationaereLeistungen { get; }
	}


	public interface IBeihilfeberechtigung
	{
		string Kennzeichnung { get; }
	}


	public interface IStationaereLeistungen
	{
		string Stationaere_Wahlleistung_Unterkunft { get; }
		decimal Prozentwert_Wahlleistung_Unterkunft { get; }
		decimal HoechstsatzWahlleistungUnterkunft { get; }
		string Stationaere_Wahlleistung_aerztliche_Behandlung { get; }
		decimal Prozentwert_Wahlleistung_aerztliche_Behandlung { get; }
		string Teilnahme_ClinicCard_Verfahren { get; }
	}
}
