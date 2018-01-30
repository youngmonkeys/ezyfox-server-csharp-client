using System;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public interface EzyObjectToMessage
	{
		EzyMessage convert(Object value);
	}
}
