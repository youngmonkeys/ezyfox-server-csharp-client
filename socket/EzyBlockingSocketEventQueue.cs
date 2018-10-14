using System;
using com.tvd12.ezyfoxserver.client.concurrent;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public abstract class EzyBlockingSocketEventQueue : EzySocketEventQueue
	{
		private readonly int capacity;
		private readonly EzyBlockingQueue<EzySocketEvent> queue;

		public EzyBlockingSocketEventQueue() : this(10000)
		{
		}

		public EzyBlockingSocketEventQueue(int capacity)
		{
			this.capacity = capacity;
			this.queue = newQueue(capacity);
		}

		protected abstract EzyBlockingQueue<EzySocketEvent> newQueue(int capacity);

		public int size()
		{
			return queue.size();
		}

		public void clear()
		{
			queue.clear();
		}

		public bool isFull()
		{
			return queue.size() >= capacity;
		}

		public bool isEmpty()
		{
			return queue.isEmpty();
		}

		public bool add(EzySocketEvent evt)
		{
			if (queue.size() >= capacity)
				return false;
			return queue.offer(evt);
		}

		public EzySocketEvent take()
		{
			EzySocketEvent evt = queue.take();
			return evt;
		}

	}
}
