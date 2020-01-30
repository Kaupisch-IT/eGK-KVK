using System;
using Newtonsoft.Json;

namespace KaupischIT.CardReader
{
	class Program
	{
		static void Main()
		{
			// Verbindung mit einem Chipkartenterminal herstellen und (falls eingesteckt) die Versichertenstammdaten einer eGK oder KVK/PKV-Card auslesen
			CardResult result = CardTerminalClient.ReadCard("ctacs.dll");

			// die ausgelesenen Versichertenstammdaten als JSON auf der Konsole ausgeben
			string json = JsonConvert.SerializeObject(result,Formatting.Indented);
			Console.WriteLine(json);

			Console.ReadKey();
		}
	}
}
