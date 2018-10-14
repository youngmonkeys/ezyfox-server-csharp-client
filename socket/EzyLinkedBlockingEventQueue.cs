using com.tvd12.ezyfoxserver.client.concurrent;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzyLinkedBlockingEventQueue : EzyBlockingSocketEventQueue
	{
		protected override EzyBlockingQueue<EzySocketEvent> newQueue(int capacity)
		{
			return new EzyBlockingQueue<EzySocketEvent>(capacity);
		}
	}
}
