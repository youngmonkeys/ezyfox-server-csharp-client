using com.tvd12.ezyfoxserver.client.evt;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public interface IEzyEventHandler
	{
	}

	public interface EzyEventHandler<E> : IEzyEventHandler where E : EzyEvent
	{
		void handle(E evt);
	}
}
