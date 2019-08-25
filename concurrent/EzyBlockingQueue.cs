using System;
using System.Threading;
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

		public override bool add(E e)
		{

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
			bool success = add(e);
			return success;
		}

		public override E peek()
		{
			lock(queue)
			{
				E e = queue.Peek();
				return e;
			}
		}

        public override E poll() 
        {
            E e = take();
            return e;
        }

        public override E take()
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

                E e = queue.Dequeue();
                return e;
            }
        }

		public override E remove()
		{
			lock(queue)
			{
				E e = queue.Dequeue();
				return e;
			}
		}

		public override int size()
		{
			lock (queue)
			{
				int count = queue.Count;
				return count;
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
			bool empty = (size() == 0);
			return empty;
		}

	}
}
