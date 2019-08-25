using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.socket;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyDataHandlers : EzyAbstractHandlers
	{
		private readonly IDictionary<Object, EzyDataHandler> handlers;

		public EzyDataHandlers(EzyClient client) : base(client)
		{
			this.handlers = new Dictionary<Object, EzyDataHandler>();
		}

		public void addHandler(Object cmd, EzyDataHandler handler)
		{
			this.configHandler(handler);
			this.handlers[cmd] = handler;
		}

		public EzyDataHandler getHandler(Object cmd)
		{
			EzyDataHandler handler = null;
			if(handlers.ContainsKey(cmd))
				handler = handlers[cmd];
			return handler;
		}

        public void handle(EzyCommand cmd, EzyArray data)
        {
            if (handlers.ContainsKey(cmd)) 
            {
                EzyDataHandler hd = handlers[cmd];
                hd.handle(data);
            }
            else
            {
                logger.warn("has no handler for command: " + cmd);
            }
        }

	}
}
