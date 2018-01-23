namespace com.tvd12.ezyfoxserver.client.io
{
	public sealed class EzyLongs
	{
		private EzyLongs()
		{

		}

		public static long bin2long(int length)
		{
			return EzyMath.bin2long(length);
		}

		public static long bin2long(byte[] bytes)
		{
			return EzyMath.bin2long(bytes);
		}

		public static long bin2ulong(byte[] bytes)
		{
			return EzyMath.bin2ulong(bytes);
		}
	}
}
