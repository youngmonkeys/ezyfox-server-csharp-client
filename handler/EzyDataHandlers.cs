using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.socket;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyDataHandlers : EzyAbstractHandlers
	{
		private readonly IDictionary<Object, EzyDataHandler> handlers;

		public EzyDataHandlers(EzyClient client, EzyPingSchedule pingSchedule) :
			base(client, pingSchedule)
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

	}
}
