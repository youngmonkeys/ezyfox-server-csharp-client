namespace com.tvd12.ezyfoxserver.client.codec
{
    public sealed class EzyMessageHeaderReader
	{
        private EzyMessageHeaderReader() 
        {
        }

        public static bool readBigSize(byte header)
		{
			return (header & 1 << 0) != 0;
		}

        public static bool readEncrypted(byte header)
		{
			return (header & (1 << 1)) != 0;
		}

        public static bool readCompressed(byte header)
		{
			return (header & (1 << 2)) != 0;
		}

        public static bool readText(byte header)
		{
			return (header & (1 << 3)) != 0;
		}

        public static bool readRawBytes(byte header)
        {
            return (header & (1 << 4)) != 0;
        }

        public static bool readUdpHandshake(byte header)
        {
            return (header & (1 << 5)) != 0;
        }

        public static bool readHasNext(byte header)
        {
            return (header & (1 << 7)) != 0;
        }

        public static EzyMessageHeader read(byte header)
		{
            return new EzySimpleMessageHeader(
                readBigSize(header),
                readEncrypted(header),
                readCompressed(header),
                readText(header),
                readRawBytes(header),
                readUdpHandshake(header));
		}
	}
}
