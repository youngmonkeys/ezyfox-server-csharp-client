using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.evt;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public abstract class EzyAbstractEventHandler<E> : 
        EzyLoggable,
		EzyEventHandler, 
		EzyClientAware where E : EzyEvent
	{
		protected EzyClient client;

		public void handle(EzyEvent evt)
		{
			process((E)evt);
		}

		protected abstract void process(E evt);

		public void setClient(EzyClient client)
		{
			this.client = client;
		}
	}
}
