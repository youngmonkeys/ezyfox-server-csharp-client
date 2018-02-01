namespace com.tvd12.ezyfoxserver.client.socket
{
	public interface EzySocketEventHandler<E> where E : EzySocketEvent
	{
		void handle(E e);
	}
}
