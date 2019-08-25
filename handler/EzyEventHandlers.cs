using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.socket;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyEventHandlers : EzyAbstractHandlers
	{
		private readonly IDictionary<Object, EzyEventHandler> handlers;

		public EzyEventHandlers(EzyClient client) : base(client)
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

        public void handle(EzyEvent evt) 
        {
            EzyEventType eventType = evt.getType();
            if (handlers.ContainsKey(eventType)) 
            {
                EzyEventHandler hd = handlers[eventType];
                hd.handle(evt);
            }
            else 
            {
                logger.warn("has no handler for event type: " + eventType);
            }
}
	}
}
