using System;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.request;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public abstract class EzyAbstractEventHandler<E> : 
		EzyEventHandler<E>, 
		EzyClientAware where E : EzyEvent
	{
		protected EzyClient client;

		public abstract void handle(E evt);

		public void setClient(EzyClient client)
		{
			this.client = client;
		}
	}
}
