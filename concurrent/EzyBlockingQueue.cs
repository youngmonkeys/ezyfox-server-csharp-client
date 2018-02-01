using System;
using System.Threading;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.concurrent
{
	public class EzyBlockingQueue<E> : EzyQueue<E>
	{

		public EzyBlockingQueue() : base()
		{
		}

		public EzyBlockingQueue(int capacity) : base(capacity)
		{
		}

		public E take()
		{
			lock (queue)
			{
				// If we have items remaining in the queue, skip over this. 
				while (queue.Count <= 0)
				{
					// Release the lock and block on this line until someone
					// adds something to the queue, resuming once they 
					// release the lock again.
					Monitor.Wait(queue);
				}

				return queue.Dequeue();
			}
		}

		public override bool add(E e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("cannot push null value to queue");
			}

			lock (queue)
			{
				if (queue.Count >= capacity)
				{
					return false;
				}
				queue.Enqueue(e);

				// If the consumer thread is waiting for an item
				// to be added to the queue, this will move it
				// to a waiting list, to resume execution
				// once we release our lock.
				Monitor.Pulse(queue);
			}
			return true;
		}

		public override bool offer(E e)
		{
			return add(e);
		}

		public override E peek()
		{
			lock(queue)
			{
				return queue.Peek();
			}
		}

		public override E remove()
		{
			lock(queue)
			{
				return queue.Dequeue();
			}
		}

		public override int size()
		{
			lock (queue)
			{
				return queue.Count;
			}
		}

		public override void clear()
		{
			lock (queue)
			{
				queue.Clear();
			}
		}

		public override bool isEmpty()
		{
			return size() == 0;
		}

	}
}
