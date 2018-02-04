using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.evt;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public interface EzyEventHandler<E> where E : EzyEvent
	{
		void handle(E evt);
	}

	public abstract class EzyAbstractEventHandler<E> : EzyLoggable, EzyEventHandler<E> where E : EzyEvent
	{
		public abstract void handle(E evt);
	}
}
