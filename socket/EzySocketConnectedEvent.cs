using System;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySocketConnectedEvent : EzySocketStatusEvent
	{
		public int getSocketStatus()
		{
			return EzySocketStatus.CONNECTED;
		}
	}
}
