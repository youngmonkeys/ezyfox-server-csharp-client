using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.manager;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public abstract class EzyAbstractDataHandler : EzyDataHandler, EzyClientAware
	{

		protected EzyClient client;
		protected EzyHandlerManager handlerManager;

		public abstract void handle(EzyArray data);

		public void setClient(EzyClient client)
		{
			this.client = client;
			this.handlerManager = client.getHandlerManager();
		}
	}

}
