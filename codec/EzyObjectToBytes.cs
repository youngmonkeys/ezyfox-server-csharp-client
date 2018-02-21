using System;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public interface EzyObjectToBytes
	{
		byte[] convert(Object value);
	}
}
