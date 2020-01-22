
namespace KaupischIT.CardReader
{
	public class CtCommand
	{
		public string Name { get; protected set; }

		public CtDestination Destination { get; protected set; }

		public byte DestinationAddress { get { return (byte)this.Destination; } }

		public byte[] Bytecode { get; protected set; }


		public enum CtDestination : byte
		{
			Card = 0,
			Terminal = 1,
		}


		public CtCommand(CtDestination destination,string name,params byte[] bytecode)
		{
			this.Name = name;
			this.Destination = destination;
			this.Bytecode = bytecode;
		}

		public override string ToString()
		{
			return this.Name;
		}


		// Kommandos an den Kartenleser

		/// <summary> Kartenleser zurücksetzen </summary>
		public static CtCommand ResetCT
			=> new CtCommand(CtDestination.Terminal,"ResetCT",0x20,0x11,0x00,0x00,0x00);

		/// <summary> Kartenanforderung </summary>
		public static CtCommand RequestICC
			=> new CtCommand(CtDestination.Terminal,"RequestICC",0x20,0x12,0x01,0x00,0x00);

		/// <summary> Karte auswerfen </summary>
		public static CtCommand EjectICC
			=> new CtCommand(CtDestination.Terminal,"EjectICC",0x20,0x15,0x01,0x00);

		// Kommandos zum Lesen einer KVK

		/// <summary> KVK-Applikation selektieren </summary>
		public static CtCommand SelectKVK
			=> new CtCommand(CtDestination.Card,"Select (KVK)",0x00,0xa4,0x04,0x00,0x06,0xd2,0x76,0x00,0x00,0x01,0x01);

		/// <summary> KVK-Template lesen </summary>
		public static CtCommand ReadKVK
			=> new CtCommand(CtDestination.Card,"Read Binary (KVK)",0x00,0xb0,0x00,0x00,0x00);

		// Kommandos zum Lesen einer eGK

		/// <summary> eGGK-Applikation selektieren </summary>
		public static CtCommand SelectEGK
			=> new CtCommand(CtDestination.Card,"Select (EGK)",0x00,0xa4,0x04,0x0c,0x06,0xd2,0x76,0x00,0x00,0x01,0x02);

		/// <summary> VST-Template lesen </summary>
		public static CtCommand ReadVST
			=> new CtCommand(CtDestination.Card,"Read Binary (VST)",0x00,0xb0,0x8c,0x00,0x00,0x00,0x00);

		/// <summary> PD-Template lesen </summary>
		public static CtCommand ReadPD
			=> new CtCommand(CtDestination.Card,"Read Binary (PD)",0x00,0xb0,0x81,0x00,0x00,0x00,0x00);

		/// <summary> VD-Template lesen </summary>
		public static CtCommand ReadVD
			=> new CtCommand(CtDestination.Card,"Read Binary (VD)",0x00,0xb0,0x82,0x00,0x00,0x00,0x00);
	}
}
