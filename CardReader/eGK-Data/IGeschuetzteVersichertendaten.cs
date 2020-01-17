namespace CardReader.Egk.GeschuetzteVersichertendaten
{
	public interface IGeschuetzteVersichertendaten
	{
		/// <summary>
		/// Gibt an, ob für den Versicherten eine Befreiung nach § 62 SGB V vorliegt.
		/// Dieses Datenfeld ist besonders schützenswert und daher nicht frei auslesbar, sondern nur berechtigten Personen zugänglich.
		/// </summary>
		IZuzahlungsstatus Zuzahlungsstatus { get; }

		/// <summary> Gibt die Teilnahme des Versicherten an einem Disease Management Program an. Die Kennzeichnung erfolgt gemäß der Schlüsseltabelle. </summary>
		string DMP_Kennzeichnung { get; }

		string CDM_VERSION { get; }
	}


	public interface IZuzahlungsstatus
	{
		/// <summary>
		/// Gibt an, ob für den Versicherten eine Befreiung nach § 62 SGB V vorliegt.
		/// Dieses Datenfeld ist besonders schützenswert und daher nicht frei auslesbar, sondern nur berechtigten Personen zugänglich.
		/// </summary>
		string Status { get; }

		/// <summary>
		/// Gibt die Gültigkeit der Befreiung von der Zuzahlungspflicht nach § 62 SGB V an. Format: YYYYMMDD
		/// Dieses Datenfeld ist besonders schützenswert und daher nicht frei auslesbar, sondern nur berechtigten Personen zugänglich.
		/// </summary>
		string Gueltig_bis { get; }
	}
}