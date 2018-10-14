using System;
using com.tvd12.ezyfoxserver.client.socket;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyAbstractHandlers
	{
		private readonly EzyClient client;
		private readonly EzyPingSchedule pingSchedule;

		public EzyAbstractHandlers(EzyClient client, EzyPingSchedule pingSchedule)
		{
			this.client = client;
			this.pingSchedule = pingSchedule;
		}

		protected void configHandler(Object handler)
		{
			if (handler is EzyClientAware)
				((EzyClientAware)handler).setClient(client);
			if (handler is EzyPingScheduleAware)
				((EzyPingScheduleAware)handler).setPingSchedule(pingSchedule);
		}
	}
}
