using com.tvd12.ezyfoxserver.client.io;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public sealed class MsgPackConstant
	{
		public static readonly long MAX_POSITIVE_FIXINT = EzyMath.bin2int(7);
		public static readonly long MAX_UINT8 = EzyMath.bin2int(8);
		public static readonly long MAX_UINT16 = EzyMath.bin2int(16);
        public static readonly long MAX_UINT32 = EzyMath.bin2long(32);

		public static readonly long MIN_NEGATIVE_FIXINT = -EzyMath.bin2int(5) - 1;
		public static readonly long MIN_INT8 = -EzyMath.bin2int(7) - 1;
		public static readonly long MIN_INT16 = -EzyMath.bin2int(15) - 1;
		public static readonly long MIN_INT32 = -EzyMath.bin2int(31) - 1;

		public static readonly int MAX_FIXMAP_SIZE = EzyMath.bin2int(4);
		public static readonly int MAX_FIXARRAY_SIZE = EzyMath.bin2int(4);
		public static readonly int MAX_ARRAY16_SIZE = EzyMath.bin2int(16);
		public static readonly int MAX_FIXSTR_SIZE = EzyMath.bin2int(5);
		public static readonly int MAX_STR8_SIZE = EzyMath.bin2int(8);
		public static readonly int MAX_STR16_SIZE = EzyMath.bin2int(16);
		public static readonly int MAX_STR32_SIZE = EzyMath.bin2int(31);
		public static readonly int MAX_MAP16_SIZE = EzyMath.bin2int(16);
		public static readonly int MAX_BIN8_SIZE = EzyMath.bin2int(8);
		public static readonly int MAX_BIN16_SIZE = EzyMath.bin2int(16);
		public static readonly int MAX_BIN32_SIZE = EzyMath.bin2int(31);

		public static readonly int MAX_SMALL_MESSAGE_SIZE = EzyMath.bin2int(16);

		private MsgPackConstant()
		{
		}
	}
}
