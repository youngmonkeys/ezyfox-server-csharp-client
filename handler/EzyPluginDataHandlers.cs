using System;
using System.Collections.Generic;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyPluginDataHandlers
	{
		private readonly IDictionary<Object, EzyPluginDataHandler> handlers;

		public EzyPluginDataHandlers()
		{
			this.handlers = new Dictionary<Object, EzyPluginDataHandler>();
		}

		public EzyPluginDataHandler getHandler(Object cmd)
		{
			EzyPluginDataHandler handler = null;
			if (handlers.ContainsKey(cmd))
				handler = handlers[cmd];
			return handler;
		}

		public void addHandler(Object cmd, EzyPluginDataHandler handler)
		{
			handlers[cmd] = handler;
		}

	}
}