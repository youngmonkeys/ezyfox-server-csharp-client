namespace com.tvd12.ezyfoxserver.client.io
{
	public sealed class EzyInts
	{
		private EzyInts()
		{
		}

		public static int bin2int(int length)
		{
			return EzyMath.bin2int(length);
		}

		public static int bin2int(byte[] bytes)
		{
			return EzyMath.bin2int(bytes);
		}

		public static int bin2uint(byte[] bytes)
		{
			return EzyMath.bin2uint(bytes);
		}
	}
}
