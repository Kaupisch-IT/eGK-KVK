﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CardReader
{
	[DebuggerDisplay("{VorName} {FamilienName}, {KrankenKassenName}")]
	public class KvkResult
	{
		private readonly Dictionary<byte,string> values;

		public KvkResult(byte[] bytes)
		{
			this.values = this.Decode(bytes);
		}


		private Dictionary<byte,string> Decode(byte[] bytes)
		{
			Dictionary<byte,string> result = new Dictionary<byte,string>();
			Encoding encoding = Encoding.GetEncoding("DIN_66003");

			int start = (bytes[0]==0x82 || bytes[0]==0x92 || bytes[0]==0xa2) ? 30 : 0;
			for (int i = start; i<bytes.Length-1-2; i++) //letzte 2 Bytes (ReturnCode) auslassen
			{
				byte tag = bytes[i++];
				int length = this.ReadLength(bytes,ref i);
				string value = encoding.GetString(bytes,i+1,length);

				if (tag!=0x60)
				{
					result.Add(tag,value);
					i += length;
				}
			}

			return result;
		}


		private int ReadLength(byte[] bytes,ref int i)
		{
			byte firstByte = bytes[i];
			if (firstByte==0x81) // 128..255
				return bytes[++i];
			else if (firstByte==0x82) // 256..65535
				return (bytes[++i]<<8) + bytes[++i];
			else // 0..127
				return firstByte;
		}


		public string this[byte tag]
			=>  (this.values.TryGetValue(tag,out string result)) ? result : null;




		/// <summary> Gibt die Bezeichnung und Ort der ausstellenden Krankenkasse an. </summary>
		public string KrankenKassenName
			=> this[0x80];

		/// <summary> Gibt die Kassennummer oder das Institutionskennzeichen an. </summary>
		public string KrankenKassenNummer
			=> this[0x81];

		/// <summary> Gibt die VKNR ("Vertragskassennummer der Kassenärztlichen Vereinigungen") bzw. das WOP-Kennzeichen ("Wohnortprinzip") an. </summary>
		public string VKNR
			=> this[0x8F];

		/// <summary> Gibt die Krankenversichertennummer an. </summary>
		public string VersichertenNummer
			=> this[0x82];

		/// <summary> 
		/// Gibt den Versichertenstatus an. 
		/// 1 = Versicherungspflichtige und -berechtigte
		/// 3 = Familienversicherte
		/// 5 = Rentner in der Krankenversicherung der Rentner und deren familienversicherten Angehörige
		/// </summary>
		public string VersichertenStatus
			=> this[0x83];

		/// <summary> Gibt das Kennzeichen bei Teilnehmern an einem Disease-Management-Programm an. </summary>
		public string StatusErgänzung
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

		/// <summary> Gibt den Familienname an.n </summary>
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
