using System;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public interface EzySocketEvent
	{
		EzySocketEventType getType();

		Object getData();
	}
}
