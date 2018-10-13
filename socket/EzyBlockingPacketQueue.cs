using System;
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
			int size = queue.size();
			return size;
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
			int size = queue.size();
			bool full = size >= capacity;
			return full;
		}

		public bool isEmpty()
		{
			bool empty = queue.isEmpty();
			return empty;
		}

		public bool add(EzyPacket packet)
		{
			int size = queue.size();
			if (size >= capacity)
				return false;
			bool success = queue.offer(packet);
			return success;
		}
	}
}
