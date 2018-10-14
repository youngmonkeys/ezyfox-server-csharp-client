using System;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public interface EzyObjectToByteEncoder
	{
		byte[] encode(Object msg);
	}
}
