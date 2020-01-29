using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace KaupischIT.CardReader
{
	public static class Extensions
	{
		/// <summary> Wandelt das übergebene Byte in eine Zeichenfolge in Hexadezimaldarstellung um </summary>
		public static string ToHexString(this byte value)
			=> Convert.ToString(value,16).PadLeft(2,'0');


		/// <summary>
		/// Ruft die Zwei-Byte-Statuswort am Ende der zurückgelieferten Antwort der CT-API in Hexadezimaldarstellung ab
		/// </summary>
		public static string GetStatusBytes(this byte[] bytes)
			=> (bytes==null || bytes.Length<2) ? null : (bytes[bytes.Length-2].ToHexString() + bytes[bytes.Length-1].ToHexString());


		/// <summary> Ruft ab, ob das Zwei-Byte-Statuswort am Ende der zurückgelieferten Antwort der CT-API einen Erfolg impliziert </summary>
		public static bool StatusIsSuccess(this byte[] bytes)
			// Das erste Byte des Trailers (= das vorletzte Byte in der Bytefolge) SW1 kodiert stets den Ausgang einer Operation
			=> (bytes!=null && bytes.Length>=2 && new[] { "90","91","92","61" }.Contains(bytes[bytes.Length-2].ToHexString())); // Process completed - Normal Processing

		/// <summary> Ruft ab, ob das Zwei-Byte-Statuswort am Ende der zurückgelieferten Antwort der CT-API eine Warnung impliziert </summary>
		public static bool StatusIsWarning(this byte[] bytes)
			// Das erste Byte des Trailers (= das vorletzte Byte in der Bytefolge) SW1 kodiert stets den Ausgang einer Operation
			=> (bytes!=null && bytes.Length>=2 && new[] { "62","63" }.Contains(bytes[bytes.Length-2].ToHexString())); // Process completed - Warning

		/// <summary> Ruft ab, ob das Zwei-Byte-Statuswort am Ende der zurückgelieferten Antwort der CT-API einen Fehler impliziert </summary>
		public static bool StatusIsError(this byte[] bytes)
			// Das erste Byte des Trailers (= das vorletzte Byte in der Bytefolge) SW1 kodiert stets den Ausgang einer Operation
			=> (bytes==null || bytes.Length<2 || new[] { "64","65","67","68","69","6a","6b","6c","6d","6e","6f" }.Contains(bytes[bytes.Length-2].ToHexString())); // Process aborted - Execution/Checking Error


		/// <summary>
		/// Überprüft das Zwei-Byte-Statuswort am Ende der zurückgelieferten Antwort der CT-API.
		/// </summary>
		/// <param name="bytes">die Bytefolge, deren Trailer ein Zwei-Byte-Statuswort(SW1-SW2) beinhaltet, dessen Kodierung in Anlehnung an ISO 7816-4 erfolgt.</param>
		/// <param name="specificReturnCodes">eine Auflistung spezifischer Status-Codes und deren Bedeutung</param>
		/// <returns>die übergebene Bytefolge</returns>
		public static byte[] CheckStatusBytes(this byte[] bytes,IDictionary<string,string> specificReturnCodes)
		{
#if DEBUG
			// den Namen der Methode ermitteln, aus der die CheckStatusBytes-Methode aufgerufen wurde
			string methodName = new StackTrace().GetFrame(1).GetMethod().Name;

			string statusBytes = bytes.GetStatusBytes();
			if (statusBytes!=null)
			{
				string responseStatus =
					(bytes.StatusIsSuccess()) ? "Process completed - Normal Processing" :
					(bytes.StatusIsWarning()) ? "Process completed - Warning" :
					(bytes.StatusIsError()) ? "Process aborted - Execution/Checking Error" : "";

				// in den Auflistungen der erwarteten Statuscodes nachschauen, ob es eine Beschreibung gibt
				string description =
					(specificReturnCodes.ContainsKey(statusBytes)) ? specificReturnCodes[statusBytes] :
					(generalStatusBytes.ContainsKey(statusBytes)) ? generalStatusBytes[statusBytes] :
					(generalStatusBytes.ContainsKey(statusBytes.Substring(0,3))) ? generalStatusBytes[statusBytes.Substring(0,3)] :
					(generalStatusBytes.ContainsKey(statusBytes.Substring(0,2))) ? generalStatusBytes[statusBytes.Substring(0,2)] : responseStatus;

				Debug.WriteLine(methodName.PadRight(12) + statusBytes + ((description!=null) ? " \t("+description+")" : ""));
			}
			else
				Debug.WriteLine(methodName.PadRight(12)+"Response Trailer vorhanden");
#endif
			return bytes;
		}

		// Auflistung der bekannten Rückgabewerte für Status-Bytes und deren Bedeutung
		private static readonly Dictionary<string,string> generalStatusBytes = new Dictionary<string,string>
		{
			{ "9000", "Normal ending of the command" },
			{ "91",   "Normal ending of the command, with extra information from the proactive UICC containing a command for the terminal" },
			{ "92",   "Normal ending of the command, with extra information concerning an ongoing data transfer session." },
			{ "9300", "SIM Application Toolkit is busy. Command cannot be executed at present, further normal commands are allowed" },
			{ "6200", "Warning: No information given, state of non volatile memory unchanged" },
			{ "6281", "Warning: Part of returned data may be corrupted" },
			{ "6282", "Warning: End of file/record reached before reading Le bytes" },
			{ "6283", "Warning: Selected file invalidated" },
			{ "6285", "Warning: Selected file in termination state" },
			{ "62f1", "Warning: More data available" },
			{ "62f2", "Warning: More data available and proactive command pending" },
			{ "62f3", "Warning: Response data available" },
			{ "63f1", "Warning: More data expected" },
			{ "63f2", "Warning: More data expected and proactive command pending" },
			{ "63c",  "Warning: Command successful but after using an internal update retry routine 'X' times - Verification failed, 'X' retries remaining(see note)" },
			{ "6400", "Error: No information given, state of non-volatile memory unchanged" },
			{ "6500", "Error: No information given, state of non-volatile memory changed" },
			{ "6581", "Error: Memory problem" },
			{ "67",   "Error: The interpretation of this status word is command dependent,except for SW2 = '00'" },
			{ "6b00", "Error: Wrong parameter(s) P1-P2" },
			{ "6f",   "Error: The interpretation of this status word is command dependent,except for SW2 = '00'" },
			{ "6800", "Error: No information given" },
			{ "6881", "Error: Logical channel not supported" },
			{ "6882", "Error: Secure messaging not supported" },
			{ "6981", "Error: Command incompatible with file structure" },
			{ "6982", "Error: Security status not satisfied" },
			{ "6983", "Error: Authentication/PIN method blocked" },
			{ "6984", "Error: Referenced data invalidated" },
			{ "6985", "Error: Conditions of use not satisfied" },
			{ "6986", "Error: Command not allowed (no EF selected)" },
			{ "6989", "Error: Command not allowed - secure channel - security not satisfied" },
			{ "6a81", "Error: Function not supported" },
			{ "6a82", "Error: File not found" },
			{ "6a83", "Error: Record not found" },
			{ "6a84", "Error: Not enough memory space" },
			{ "6a86", "Error: Command parameters not supported" },
			{ "6a87", "Error: Lc inconsistent with P1 to P2" },
			{ "64a1", "Error: Keine Karte vorhanden" },
			{ "64a2", "Error: Karte nicht aktiviert" },
			{ "6700", "Error: Falsche Länge / Zu viele oder zu wenig Daten (Objekte) im Kommando enthalten." },
			{ "6900", "Error: Kommando (ggf. z.Zt.) nicht zulässig" },
			{ "6930", "Error: Das Terminal unterstützt (für diesen Befehl) keine Timer-Option." },
			{ "6940", "Error: Das Terminal unterstützt (für diesen Befehl) keine Display-Option." },
			{ "6941", "Error: Funktionseinheit belegt / nicht verfügbar" },
			{ "6942", "Error: Das Display unterstützt den selektierten Zeichensatz nicht." },
			{ "6a00", "Error: Falsche Parameter P1, P2" },
			{ "6a80", "Error: Unzulässige Parameter (Datenobjekt) im Datenfeld" },
			{ "6a88", "Error: Referenzdaten (Datenobjekt) nicht in Datenteil des Kommandos enthalten." },
			{ "6a89", "Error: Zu viele Datenobjekte gleichen Typs sind enthalten." },
			{ "6c00", "Error: Falsche Längenangabe Le" },
			{ "6d00", "Error: Falsches/unbekanntes Instruction-Byte" },
			{ "6e00", "Error: Falsches/unbekanntes Class-Byte" },
			{ "6f00", "Error: Kommunikationsfehler zur Karte" },
		};
	}
}
