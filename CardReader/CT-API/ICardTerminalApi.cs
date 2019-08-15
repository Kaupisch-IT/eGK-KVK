
namespace CardReader
{
	public interface ICardTerminalApi
	{
		sbyte Init(ushort ctn,ushort pn);
		sbyte Close(ushort ctn);
		sbyte Data(ushort ctn,ref byte dad,ref byte sad,ushort lenc,ref byte command,ref ushort ulenr,ref byte response);
	}
}
