using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyAppDataHandlers
	{
		private readonly IDictionary<Object, IEzyAppDataHandler> handlers;

		public EzyAppDataHandlers()
		{
			this.handlers = new Dictionary<Object, IEzyAppDataHandler>();
		}

		public void addHandler(Object cmd, IEzyAppDataHandler handler)
		{
			handlers[cmd] = handler;
		}

		public EzyAppDataHandler<D> getHandler<D>(Object cmd) where D : EzyData
		{
			IEzyAppDataHandler handler = null;
			if (handlers.ContainsKey(cmd))
				handler = handlers[cmd];
			return (EzyAppDataHandler<D>)handler;
		}
	}
}