using System;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzySocketDataEventHandlingLoop : EzyLoggable, EzyStartable
	{
		private volatile bool active = true;
		private readonly EzySocketDataEventHandler socketDataEventHandler;

		public EzySocketDataEventHandlingLoop(EzySocketDataEventHandler socketDataEventHandler)
		{
			this.socketDataEventHandler = socketDataEventHandler;
		}

		public void setActive(bool active)
		{
			this.active = active;
		}

		public void start()
		{
            logger.info("socket data event handling loop start");
			while (active)
			{
				socketDataEventHandler.handleEvent();
			}
		}
	}
}
