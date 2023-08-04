using System;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public interface EzyObjectToByteEncoder
	{
		byte[] encode(Object msg);

        byte[] toMessageContent(Object data);

        byte[] encryptMessageContent(byte[] messageContent, byte[] encryptionKey);
    }
}
