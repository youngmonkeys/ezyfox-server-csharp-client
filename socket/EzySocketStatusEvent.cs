using System;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public interface EzySocketStatusEvent : EzySocketEvent
	{
		int getSocketStatus();
	}
}
