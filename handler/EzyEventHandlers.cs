using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.socket;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyEventHandlers : EzyAbstractHandlers
	{
		private readonly IDictionary<Object, Object> handlers;

		public EzyEventHandlers(EzyClient client, EzyPingSchedule pingSchedule) :
				base(client, pingSchedule)
		{
			this.handlers = new Dictionary<Object, Object>();
		}

		public void addHandler(Object eventType, IEzyEventHandler handler)
		{
			this.configHandler(handler);
			handlers[eventType] = handler;
		}

		public EzyEventHandler<E> getHandler<E>(Object eventType) where E : EzyEvent
		{
			Object handler = null;
			if (handlers.ContainsKey(eventType))
				handler = handlers[eventType];
			return (EzyEventHandler<E>)handler;
		}
	}
}
