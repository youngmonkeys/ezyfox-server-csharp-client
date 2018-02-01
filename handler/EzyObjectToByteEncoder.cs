using System;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public interface EzyObjectToByteEncoder
	{
		byte[] encode(Object msg);
	}
}
