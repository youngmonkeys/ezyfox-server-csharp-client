using com.tvd12.ezyfoxserver.client.io;

namespace com.tvd12.ezyfoxserver.client.codec
{
    public sealed class EzyMessageReaders
    {
        private EzyMessageReaders() 
        { 
        }

        public static EzyMessage bytesToMessage(byte[] bytes)
        {
            EzyMessageHeader header = EzyMessageHeaderReader.read(bytes[0]);
            int messageSizeLength = header.isBigSize() ? 4 : 2;
            int minSize = 2 + messageSizeLength;
            if (bytes.Length < minSize)
                return null;
            byte[] messageSizeBytes = EzyBytes.copyBytes(bytes, 1, messageSizeLength);
            int messageSize = EzyInts.bin2int(messageSizeBytes);
            int allSize = 1 + messageSizeLength + messageSize;
            if (bytes.Length != allSize)
                return null;
            int contentStart = 1 + messageSizeLength;
            byte[] messageContent = EzyBytes.copyBytes(bytes, contentStart, messageSize);
            return new EzySimpleMessage(header, messageContent, messageSize);
        }
    }
}
