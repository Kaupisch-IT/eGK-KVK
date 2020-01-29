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
		{
			return Convert.ToString(value,16).PadLeft(2,'0');
		}


		/// <summary>
		/// Ruft die Zwei-Byte-Statuswort am Ende der zurückgelieferten Antwort der CT-API in Hexadezimaldarstellung ab
		/// </summary>
		public static string GetStatusBytes(this byte[] bytes)
		{
			if (bytes==null || bytes.Length<2)
				return null;
			else
				return bytes[bytes.Length-2].ToHexString()+bytes[bytes.Length-1].ToHexString();
		}


		/// <summary>
		/// Überprüft das Zwei-Byte-Statuswort am Ende der zurückgelieferten Antwort der CT-API.
		/// </summary>
		/// <param name="bytes">die Bytefolge, deren Trailer ein Zwei-Byte-Statuswort(SW1-SW2) beinhaltet, dessen Kodierung in Anlehnung an ISO 7816-4 erfolgt.</param>
		/// <param name="specificReturnCodes">eine Auflistung spezifischer Status-Codes und deren Bedeutung</param>
		/// <returns>die übergebene Bytefolge</returns>
		public static byte[] CheckStatusBytes(this byte[] bytes,IDictionary<string,string> specificReturnCodes)
		{
#if DEBUG
			Dictionary<string,string> generalStatusBytes = new Dictionary<string,string>
			{
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

			// den Namen der Methode ermitteln, aus der die CheckStatusBytes-Methode aufgerufen wurde
			string methodName = new StackTrace().GetFrame(1).GetMethod().Name;

			string statusBytes = bytes.GetStatusBytes();
			if (statusBytes!=null)
			{
				/// Das erste Byte des Trailers (= das vorletzte Byte in der Bytefolge) SW1 kodiert stets den Ausgang einer Operation
				string sw1 = statusBytes.Substring(0,2);
				string responseStatus =
					(new[] { "90","61" }.Contains(sw1)) ? "Process completed - Normal Processing" :
					(new[] { "62","63" }.Contains(sw1)) ? "Process completed - Warning" :
					(new[] { "64","65" }.Contains(sw1)) ? "Process aborted - Execution Error" :
					(new[] { "67","68","69","6a","6b","6c","6d","6e","6f" }.Contains(sw1)) ? "Process aborted - Checking Error" : "";

				// in den Auflistungen der erwarteten Statuscodes nachschauen, ob es eine Beschreibung gibt
				string description = ((specificReturnCodes.TryGetValue(statusBytes,out string value) || generalStatusBytes.TryGetValue(statusBytes,out value)) ? value : responseStatus);

				Debug.WriteLine(methodName.PadRight(12) + statusBytes + ((description!=null)?" \t("+description+")":""));
			}
			else
				Debug.WriteLine(methodName.PadRight(12)+"Response Trailer vorhanden");
#endif
			return bytes;
		}
	}
}
