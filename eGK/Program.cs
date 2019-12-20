using System.Windows;
using CardReader;
using CardReader.Results;
using Newtonsoft.Json;

namespace eGK
{
	class Program
	{
		static void Main()
		{
			using (CardTerminalClient cardTerminalClient = new CardTerminalClient("ctacs.dll"))
			{
				cardTerminalClient.ResetCT();

				cardTerminalClient.RequestICC();
				cardTerminalClient.SelectEGK();
				EgkResult egkResult = cardTerminalClient.ReadEGK();

				string json = JsonConvert.SerializeObject(egkResult);
				MessageBox.Show(json);

				cardTerminalClient.EjectICC();
			}
		}
	}
}
