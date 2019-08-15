
namespace CardReader
{
	public enum CtReturnCode : sbyte
	{
		OK           = 0,       // succeded
		ERR_INVALID  = -1,      // invalid argument
		ERR_CT       = -8,      // card terminal error
		ERR_TRANS    = -10,     // transmission error
		ERR_MEMORY   = -11,     // memory error
		ERR_HOST     = -127,    // function would be cancelled
		ERR_HTSI     = -128,    // HTSi error
	}
}
