namespace CardReader.Egk.AllgemeineVersicherungsdaten
{
	public interface IAllgemeineVersicherungsdaten
	{
		/// <summary> Die Versicherten-ID ist der 10-stellige unveränderliche Teil der 30-stelligen Krankenversichertennummer. </summary>
		IVersicherter Versicherter { get; }

		string CDM_VERSION { get; }
	}


	public interface IVersicherter
	{
		/// <summary>
		/// Gibt den Beginn des Versicherungsschutzes (hier: Leistungsanspruch) des Versicherten bei dem unter Klasse Kostenträger angegebenen Kostenträger an. 
		/// Format: YYYYMMDD (ISO-8601)
		/// </summary>
		IVersicherungsschutz Versicherungsschutz { get; }

		/// <summary>
		/// Das Informationsmodell VSD beinhaltet Daten für GKV und PKV.
		/// Für die Abbildung des definitiven Datensatzes der VSD wird aber nur die jeweils zutreffende Klasse GKV oder PKV realisiert, d.h. es erfolgte keine Abbildung von PKV-Informationen in den VSD der GKV und umgekehrt.
		/// </summary>
		IZusatzinfos Zusatzinfos { get; }
	}


	public interface IVersicherungsschutz
	{
		/// <summary>
		/// Gibt den Beginn des Versicherungsschutzes (hier: Leistungsanspruch) des Versicherten bei dem unter Klasse Kostenträger angegebenen Kostenträger an. 
		/// Format: YYYYMMDD (ISO-8601)
		/// </summary>
		string Beginn { get; }

		/// <summary>
		/// Gibt das Ende des Versicherungsschutzes (hier: Leistungsanspruch) des Ver-sicherten bei dem unter Klasse Kostenträger angegebenen Kostenträger an. 
		/// Format: YYYYMMDD (ISO-8601)
		/// </summary>
		string Ende { get; }

		/// <summary> Gibt den Kostenträger des Versicherten an. Es handelt sich um das bundesweit gültige Institutionskennzeichen (IK) des jeweiligen Kostenträgers. </summary>
		IKostentraeger Kostentraeger { get; }
	}


	public interface IKostentraeger
	{
		/// <summary> Gibt den Kostenträger des Versicherten an. Es handelt sich um das bundesweit gültige Institutionskennzeichen (IK) des jeweiligen Kostenträgers. </summary>
		string Kostentraegerkennung { get; }

		/// <summary> Gibt den Kostenträgerländercode vom Kostenträger des Versicherten an </summary>
		string Kostentraegerlaendercode { get; }

		/// <summary> Gibt den Namen der Institution bzw. Organisation an. </summary>
		string Name { get; }

		/// <summary> Identifiziert den abrechnenden Kostenträger. Für diesen sind die Kostenträgerkennung (IK) und der Name des Kostenträgers anzugeben. </summary>
		IKostentraeger AbrechnenderKostentraeger { get; }
	}


	public interface IZusatzinfos
	{
		IZusatzinfosGKV ZusatzinfosGKV { get; }
	}


	public interface IZusatzinfosGKV
	{
		/// <summary> Gibt die Versichertenart (Mitglied, Familienversicherter oder Rentner ) des Versicherten gemäß Schlüsseltabelle an. </summary>
		string Versichertenart { get; }

		/// <summary> Gibt an, ob der Versicherte die Kostenerstattung für ambulante Behandlung nach § 13 SGB V gewählt hat. </summary>
		IZusatzinfos_Abrechnung_GKV Zusatzinfos_Abrechnung_GKV { get; }
	}


	public interface IZusatzinfos_Abrechnung_GKV
	{
		/// <summary> Das Kennzeichen WOP ist gemäß § 2 Abs. 2 der Vereinbarung zur Festset-zung des Durchschnittsbetrages gemäß Artikel 2 § 2 Abs. 2 des Gesetzes zur Einführung des Wohnortprinzips bei Honorarvereinbarungen für Ärzte und Zahnärzte und zur Krankenversichertenkarte gemäß § 291 Abs. 2 Fünftes So-zialgesetzbuch (SGB V) erforderlich </summary>
		string WOP { get; }
	}


	public interface IZusatzinfosPKV
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
		string PKV_Verbandstarif { get; }

		/// <summary>
		/// Gibt den Anspruch des Versicherten auf Beihilfe an. Mögliche Werte:
		/// 01 = Beihilfe
		/// 02 = Postbeamtenkrankenkasse (PBeaKK)
		/// 03 = Krankenversorgung der Bundesbahnbeamten (KVB)
		/// </summary>
		IBeihilfeberechtigung Beihilfeberechtigung { get; }

		IStationaereLeistungen[] StationaereLeistungen { get; }
	}


	public interface IBeihilfeberechtigung
	{
		/// <summary>
		/// Gibt den Anspruch des Versicherten auf Beihilfe an. Mögliche Werte:
		/// 01 = Beihilfe
		/// 02 = Postbeamtenkrankenkasse (PBeaKK)
		/// 03 = Krankenversorgung der Bundesbahnbeamten (KVB)
		/// </summary>
		string Kennzeichnung { get; }
	}


	public interface IStationaereLeistungen
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
		string Stationaere_Wahlleistung_Unterkunft { get; }

		/// <summary> Gibt den Leistungssatz in Prozent an. Wertebereich 000-100. </summary>
		decimal Prozentwert_Wahlleistung_Unterkunft { get; }

		/// <summary> Gibt den Höchstbetrag der Erstattung für die Wahlleistung Unterkunft an. </summary>
		decimal HoechstsatzWahlleistungUnterkunft { get; }

		/// <summary>
		/// Gibt den Umfang der ärztlichen Behandlung im stationären Bereich an.
		/// 01 = Gesondert berechenbare Leistungen (Chefarztbehandlung)
		/// 02 = Allgemeine ärztliche Krankenhausleistung
		/// </summary>
		string Stationaere_Wahlleistung_aerztliche_Behandlung { get; }

		/// <summary> Gibt den Leistungssatz in Prozent an. Wertebereich 000-100. </summary>
		decimal Prozentwert_Wahlleistung_aerztliche_Behandlung { get; }

		/// <summary> Gibt die Teilnahme des Kostenträgers des Versicherten am ClinicCard-Verfahren an. </summary>
		string Teilnahme_ClinicCard_Verfahren { get; }
	}
}