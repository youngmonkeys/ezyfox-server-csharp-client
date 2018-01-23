namespace com.tvd12.ezyfoxserver.client.codec
{
	public sealed class MsgPackType
	{
		public const int POSITIVE_FIXINT = 0;
		public const int FIXMAP = 1;
		public const int FIXARRAY = 2;
		public const int FIXSTR = 3;
		public const int NIL = 4;
		public const int NEVER_USED = 5;
		public const int FALSE = 6;
		public const int TRUE = 7;
		public const int BIN8 = 8;
		public const int BIN16 = 9;
		public const int BIN32 = 10;
		public const int EXT8 = 11;
		public const int EXT16 = 12;
		public const int EXT32 = 13;
		public const int FLOAT32 = 14;
		public const int FLOAT64 = 15;
		public const int UINT8 = 16;
		public const int UINT16 = 17;
		public const int UINT32 = 18;
		public const int UINT64 = 19;
		public const int INT8 = 20;
		public const int INT16 = 21;
		public const int INT32 = 22;
		public const int INT64 = 23;
		public const int FIXEXT1 = 24;
		public const int FIXEXT2 = 25;
		public const int FIXEXT4 = 26;
		public const int FIXEXT8 = 27;
		public const int FIXEXT16 = 28;
		public const int STR8 = 29;
		public const int STR16 = 30;
		public const int STR32 = 31;
		public const int ARRAY16 = 32;
		public const int ARRAY32 = 33;
		public const int MAP16 = 34;
		public const int MAP32 = 35;
		public const int NEGATIVE_FIXINT = 36;

		private MsgPackType()
		{
		}
	}
}
