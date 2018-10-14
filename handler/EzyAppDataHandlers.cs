using System;
using System.Collections.Generic;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyAppDataHandlers
	{
		private readonly IDictionary<Object, EzyAppDataHandler> handlers;

		public EzyAppDataHandlers()
		{
			this.handlers = new Dictionary<Object, EzyAppDataHandler>();
		}

		public EzyAppDataHandler getHandler(Object cmd)
		{
			EzyAppDataHandler handler = null;
			if (handlers.ContainsKey(cmd))
				handler = handlers[cmd];
			return handler;
		}

		public void addHandler(Object cmd, EzyAppDataHandler handler)
		{
			handlers[cmd] = handler;
		}

	}
}