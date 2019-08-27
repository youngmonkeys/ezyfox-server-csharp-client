using System.Threading;

namespace com.tvd12.ezyfoxserver.client.concurrent
{
    public class EzyBlockingQueue<E> : EzySynchronizedQueue<E>
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

        public void wakeup() 
        {
            lock(queue)
            {
                queue.Enqueue(default(E));
                Monitor.Pulse(queue);
            }
        }

	}
}
