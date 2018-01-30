using System;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public interface EzyNioObjectToByteEncoder
	{
		byte[] encode(Object msg);
	}
}
