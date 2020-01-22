using System;
using System.Diagnostics.CodeAnalysis;

namespace KaupischIT.CardReader
{
	/// <summary>
	///  Die Ausnahme, die ausgelöst wird, wenn ein Fehler beim Aufruf von Card-Terminal-Funktionen auftritt
	/// </summary>
	[Serializable]
	[SuppressMessage("Usage","CA2229:Implement serialization constructors")]
	[SuppressMessage("Design","CA1032:Implement standard exception constructors")]
	public class CtException : Exception
	{
		/// <summary> Ruft den Fehlercode ab. </summary>
		public sbyte ErrorCode { get; private set; }

		/// <summary>
		/// Initialisiert eine neue Instanz einer Ausnahme, die ausgelöst wird, wenn ein Fehler beim Aufruf von Card-Terminal-Funktionen auftritt
		/// </summary>
		/// <param name="errorCode">der Fehlercode</param>
		/// <param name="message">Die Fehlermeldung, in der die Ursache der Ausnahme erklärt wird.</param>
		public CtException(sbyte errorCode,string message) : base(message) => this.ErrorCode = errorCode;
	}
}
