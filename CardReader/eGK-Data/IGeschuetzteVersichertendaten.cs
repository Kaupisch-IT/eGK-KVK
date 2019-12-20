namespace CardReader.Results.Egk.GeschuetzteVersichertendaten
{
	public interface IGeschuetzteVersichertendaten
	{
		IZuzahlungsstatus Zuzahlungsstatus { get; }
		string DMP_Kennzeichnung { get; }
		string CDM_VERSION { get; }
	}


	public interface IZuzahlungsstatus
	{
		string Status { get; }
		string Gueltig_bis { get; }
	}
}