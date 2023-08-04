using System;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public interface EzyObjectToMessage
	{
		EzyMessage convert(Object value);

        byte[] convertToMessageContent(Object value);

        EzyMessage packToMessage(byte[] content, bool encrypted);
    }
}
