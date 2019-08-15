
namespace CardReader.Commands
{
	public class Command
	{
		public string Name { get; protected set; }
		public Destination Destination { get; protected set; }
		public byte DestinationAddress { get { return (byte)this.Destination; } }
		public byte[] Bytecode { get; protected set; }

		public Command(Destination destination,string name,params byte[] bytecode)
		{
			this.Name = name;
			this.Destination = destination;
			this.Bytecode = bytecode;
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}
