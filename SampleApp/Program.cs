using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace KaupischIT.CardReader
{
	class Program
	{
		[SuppressMessage("Design","CA1031:Do not catch general exception types")]
		static void Main()
		{
			CardResult result = new CardResult();

			using (CardTerminalClient cardTerminalClient = new CardTerminalClient("ctacs.dll"))
			{
				cardTerminalClient.ResetCT();
				cardTerminalClient.RequestICC();

				try
				{
					if (!cardTerminalClient.SelectEGK().StatusIsError())
						result.EgkResult = cardTerminalClient.ReadEGK();
				}
				catch (CtException) { }

				try
				{
					if (!cardTerminalClient.SelectKVK().StatusIsError())
						result.KvKResult = cardTerminalClient.ReadKVK();
				}
				catch (CtException) { }

				cardTerminalClient.EjectICC();
			}

			string json = JsonConvert.SerializeObject(result,Formatting.Indented);
			Console.WriteLine(json);

			Console.ReadKey();
		}
	}
}
