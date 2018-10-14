using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.socket;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyEventHandlers : EzyAbstractHandlers
	{
		private readonly IDictionary<Object, EzyEventHandler> handlers;

		public EzyEventHandlers(EzyClient client, EzyPingSchedule pingSchedule) :
				base(client, pingSchedule)
		{
			this.handlers = new Dictionary<Object, EzyEventHandler>();
		}

		public EzyEventHandler getHandler(Object eventType)
		{
			EzyEventHandler handler = null;
			if (handlers.ContainsKey(eventType))
				handler = handlers[eventType];
			return handler;
		}

		public void addHandler(Object eventType, EzyEventHandler handler)
		{
			this.configHandler(handler);
			handlers[eventType] = handler;
		}
	}
}
