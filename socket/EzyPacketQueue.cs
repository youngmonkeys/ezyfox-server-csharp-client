namespace com.tvd12.ezyfoxserver.client.socket
{
	public interface EzyPacketQueue
	{
		int size();

		void clear();

		EzyPacket take();

		bool isFull();

		bool isEmpty();

		bool add(EzyPacket packet);
	}
}
