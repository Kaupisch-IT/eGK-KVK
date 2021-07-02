using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KaupischIT.CardReader
{
	/// <summary>
	/// Stellt Daten Card für Privatversicherte (PKV-Card) bereit
	/// </summary>
	[DebuggerDisplay("{VorName} {FamilienName}, {KrankenKassenName}")]
	public class PkvResult
	{
		private readonly Dictionary<byte,string> values; // beinhaltet die ausgelesenen Schlüssel-Wert-Paare der KVK-Daten


		/// <summary>
		/// Initialisiert eine neue Instanz der KvkResult-Klasse und dekodiert die übergebenen Krankenversichertendaten
		/// </summary>
		/// <param name="bytes">die Rohdaten aus dem KVK-Template der Krankenversichertenkarte</param>
		public PkvResult(byte[] bytes)
		{
			this.values = this.Decode(bytes);
		}


		/// <summary>
		/// Dekodiert die in den Rohdaten enthaltenen Schlüssel-Wert-Paare mit den Krankenversichertendaten einer KVK
		/// </summary>
		private Dictionary<byte,string> Decode(byte[] bytes)
		{
			// Die Rohdaten enthalten sowohl Metadaten (Tag, Länge) als auch die eigentlichen Nutzdaten 
			// Die Nutzdaten sind als Schlüssel-Wert-Paare abgelegt
			Dictionary<byte,string> result = new Dictionary<byte,string>();
			Encoding encoding = Encoding.GetEncoding("DIN_66003");

			// Falls das erste Byte 82, 92 oder A2 ist (jeweils Hexadezimaldarstellung), kommt als erstes ATR und Directory. Die ersten 30 Bytes können dann ignoriert/übersprungen werden.
			int start = (bytes[0]==0x82 || bytes[0]==0x92 || bytes[0]==0xa2) ? 30 : 0;

			// dann kommen die eigentlichen Daten
			for (int i = start; i<bytes.Length-1-2; i++) //letzte 2 Bytes (ReturnCode) auslassen
			{
				// als Codierungstechnik für Datenobjekte werden die "Basic Encoding Rules (BER)" der ISO-Codierungskonvention "Abstract Syntax Notation One (ASN.1)" verwendet.
				// Ein Datenobjekt besteht demnach aus:
				byte tag = bytes[i++];                                  // einem Datenobjekt-Kennzeichen ("Tag")
				int length = this.ReadLength(bytes,ref i);              // einer Längenangabe ("Length") und
				string value = encoding.GetString(bytes,i+1,length);    // einem Datenobjekt-Wert("Value").

				// alle gefundenen Schlüssel-Wert-Paare ablegen
				if (tag!=0x60) // VersichertenDatenTemplate (Container-Element) ignorieren
				{
					result.Add(tag,value);
					i += length;
				}
			}

			return result;
		}


		/// <summary>
		/// Ermittelt die Längenangabe im den KVK-Rohdaten (in einem bis drei Bytes codiert) und ändert den übergebenen Zeiger um das entsprechende Offset
		/// </summary>
		private int ReadLength(byte[] bytes,ref int i)
		{
			// Die Länge ist in 1-3 Bytes codiert:
			//	Length 0 .. 127: one byte coding the length
			//	Length 128 .. 255: 1st byte: bit b8 = 1, b7-b1= 0000001 (number of subsequent length bytes); 2nd byte: Length
			//	Length 256 .. 65535: 1st byte: bit b8 = 1,b7-b1= 0000010; 2nd + 3rd byte: Length

			byte firstByte = bytes[i];
			if (firstByte==0x81) // 128..255
				return bytes[++i];
			else if (firstByte==0x82) // 256..65535
				return (bytes[++i]<<8) + bytes[++i];
			else // 0..127
				return firstByte;
		}


		/// <summary>
		/// Ruft den Wert mit dem angegebenen Schlüssel/Tag aus den Krankenversichertendaten ab
		/// </summary>
		private string this[byte tag]
			=> (this.values.TryGetValue(tag,out string result)) ? result : null;



		/// <summary> Gibt die Bezeichnung und Ort der ausstellenden Krankenkasse an. </summary>
		public string KrankenKassenName
			=> this[0x80];

		/// <summary> Konstante 1 (ausgenullt) </summary>
		public string Konstante1
			=> this[0x81];

		/// <summary> Konstante 2 (ausgenullt) </summary>
		public string VKNR
			=> this[0x8F];

		/// <summary> Gibt die Versicherungsnummer (Vertragsnummer) an. </summary>
		public string VersicherungsNummer
			=> this[0x82];

		/// <summary> 
		/// Gibt die Personennummer (Vertragsunternummer der versicherten Person) an. 
		/// </summary>
		public string PersonenNummer
			=> this[0x83];

		/// <summary> Konstante 3 (wird mit einer "1" belegt) </summary>
		public string Konstante3
			=> this[0x90];

		/// <summary> Gibt den Titel (optional) an. </summary>
		public string Titel
			=> this[0x84];

		/// <summary> Gibt den Vornamen an. </summary>
		public string VorName
			=> this[0x85];

		/// <summary> Gibt den Namenszusatz (optional) an. </summary>
		public string NamensZusatz_VorsatzWort
			=> this[0x86];

		/// <summary> Gibt den Familienname an. </summary>
		public string FamilienName
			=> this[0x87];

		/// <summary> Gibt das Geburtsdatum an. </summary>
		public string GeburtsDatum
			=> this[0x88];

		/// <summary> Gibt die Anschrift (Straße & Hausnr.) an. </summary>
		public string StraßenName_HausNummer
			=> this[0x89];

		/// <summary> Gibt den Ländercode (optional, wenn nicht vorhanden: Deutschland) an. </summary>
		public string WohnsitzLänderCode
			=> this[0x8A];

		/// <summary> Gibt die Anschrift (Postleitzahl) an. </summary>
		public string Postleitzahl
			=> this[0x8B];

		/// <summary> Gibt die Anschrift (Ort) an. </summary>
		public string OrtsName
			=> this[0x8C];

		/// <summary> Gibt bei befristeter Gültigkeit der Karte den Monat des Fristablaufs an. Üblich bei ausreichend langer Mitgliedschaft sind Gültigkeitsdauern von fünf bis zehn Jahren oder auch mehr. </summary>	
		public string GültigkeitsDatum
			=> this[0x8D];
	}
}
