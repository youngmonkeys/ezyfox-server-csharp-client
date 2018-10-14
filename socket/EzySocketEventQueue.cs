namespace com.tvd12.ezyfoxserver.client.socket
{
	public interface EzySocketEventQueue
	{
		int size();

		void clear();

		bool isFull();

		bool isEmpty();

		bool add(EzySocketEvent evt);

		EzySocketEvent take();
	}
}
