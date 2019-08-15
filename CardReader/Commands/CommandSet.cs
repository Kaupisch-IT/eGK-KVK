
namespace CardReader.Commands
{
	public static class CommandSet
	{
		public static Command ResetCT
		{
			get { return new Command(Destination.Terminal,"ResetCT",0x20,0x11,0x00,0x00,0x00); }
		}

		public static Command RequestICC
		{
			get { return new Command(Destination.Terminal,"RequestICC",0x20,0x12,0x01,0x00,0x00); }
		}

		public static Command EjectICC
		{
			get { return new Command(Destination.Terminal,"EjectICC",0x20,0x15,0x01,0x00); }
		}

		public static Command GetStatusCtmdo
		{
			get { return new Command(Destination.Terminal,"GetStatus CTMDO",0x20,0x13,0x00,0x46,0x00); }
		}

		public static Command GetStatusIccdo
		{
			get { return new Command(Destination.Terminal,"GetStatus ICCDO",0x20,0x13,0x00,0x80,0x00); }
		}

		public static Command GetStatusFudo
		{
			get { return new Command(Destination.Terminal,"GetStatus FUDO",0x20,0x13,0x00,0x81,0x00); }
		}
		

		public static Command SelectKVK
		{
			get { return new Command(Destination.Card,"Select KVK",0x00,0xa4,0x04,0x00,0x06,0xd2,0x76,0x00,0x00,0x01,0x01); }
		}

		public static Command ReadKVK
		{
			get { return new Command(Destination.Card,"Read KVK",0x00,0xb0,0x00,0x00,0x00); }
		}


		public static Command SelectEGK
		{
			get { return new Command(Destination.Card,"Select EGK",0x00,0xa4,0x04,0x0c,0x06,0xd2,0x76,0x00,0x00,0x01,0x02); }
		}

		public static Command ReadVST
		{
			get { return new Command(Destination.Card,"Read VST",0x00,0xb0,0x8c,0x00,0x00,0x00,0x00); }
		}

		public static Command ReadPD
		{
			get { return new Command(Destination.Card,"Read PD",0x00,0xb0,0x81,0x00,0x00,0x00,0x00); }
		}

		public static Command ReadVD
		{
			get { return new Command(Destination.Card,"Read VD",0x00,0xb0,0x82,0x00,0x00,0x00,0x00); }
		}
	}
}
