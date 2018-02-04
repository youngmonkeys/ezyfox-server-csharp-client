using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.api;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyEventHandlers
	{
		private readonly IDictionary<int, Object> handlers;

		public EzyEventHandlers()
		{
			this.handlers = new Dictionary<int, Object>();
		}

		public void handleEvent<E>(E evt) where E : EzyEvent
		{
			var eventType = evt.getType();
			var handler = handlers[eventType];
			if (handler != null)
			{
				((EzyEventHandler<E>)handler).handle(evt);
			}
		}

		public void addEventHandler<E>(int eventType, Object handler) where E : EzyEvent
		{
			handlers[eventType] = handler;
		}
	}
}
