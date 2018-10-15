using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public abstract class EzyAbstractSocketEventHandler : 
        EzyLoggable, 
        EzySocketEventHandler
	{
		public abstract void handleEvent();

		public virtual void reset()
		{
		}

		public virtual void destroy()
		{
		}
	}
}
