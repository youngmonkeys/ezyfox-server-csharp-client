using System;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.socket;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyAbstractHandlers
	{
        protected readonly EzyClient client;
        protected readonly EzyLogger logger;

		public EzyAbstractHandlers(EzyClient client)
		{
			this.client = client;
            this.logger = EzyLoggerFactory.getLogger(GetType());
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
