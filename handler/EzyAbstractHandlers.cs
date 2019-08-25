using System;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.socket;

namespace com.tvd12.ezyfoxserver.client.handler
{
    public class EzyAbstractHandlers : EzyLoggable
	{
        protected readonly EzyClient client;

        public EzyAbstractHandlers(EzyClient client)
		{
			this.client = client;
		}

		protected void configHandler(Object handler)
		{
			if (handler is EzyClientAware)
				((EzyClientAware)handler).setClient(client);
			if (handler is EzyPingScheduleAware)
                ((EzyPingScheduleAware)handler).setPingSchedule(client.getPingSchedule());
		}
	}
}
