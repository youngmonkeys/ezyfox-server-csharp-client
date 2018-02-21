using System;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public interface EzyObjectSerializer
	{

		byte[] serialize(Object value);

	}
}