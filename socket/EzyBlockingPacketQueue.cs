using com.tvd12.ezyfoxserver.client.concurrent;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzyBlockingPacketQueue : EzyPacketQueue
	{
		private readonly int capacity;
		private readonly EzyBlockingQueue<EzyPacket> queue;

		public EzyBlockingPacketQueue() : this(10000)
		{
		}

		public EzyBlockingPacketQueue(int capacity)
		{
			this.capacity = capacity;
			this.queue = new EzyBlockingQueue<EzyPacket>();
		}

		public int size()
		{
			int queueSize = queue.size();
            return queueSize;
		}

		public void clear()
		{
			queue.clear();
		}

		public EzyPacket take()
		{
			EzyPacket packet = queue.take();
			return packet;
		}


		public bool isFull()
		{
            int queueSize = queue.size();
            bool full = queueSize >= capacity;
			return full;
		}

		public bool isEmpty()
		{
			bool empty = queue.isEmpty();
			return empty;
		}

		public bool add(EzyPacket packet)
		{
            int queueSize = queue.size();
            if (queueSize >= capacity)
				return false;
			bool success = queue.offer(packet);
			return success;
		}

        public void wakeup()
        {
            queue.wakeup();    
        }
	}
}
