using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public interface EzySocketEventHandler : EzyDestroyable, EzyResettable
	{
		void handleEvent();
	}
}
