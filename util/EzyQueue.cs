using System;
using System.Collections.Generic;

namespace com.tvd12.ezyfoxserver.client.util
{
	public class EzyQueue<E>
	{
		protected readonly int capacity;
		protected readonly Queue<E> queue;

		public EzyQueue() : this(Int32.MaxValue)
		{
		}

		public EzyQueue(int capacity)
		{
			this.capacity = capacity;
			this.queue = new Queue<E>();
		}

		public virtual bool add(E e)
		{
			if (queue.Count >= capacity)
			{
				return false;
			}
			queue.Enqueue(e);
			return true;
		}

		public virtual bool offer(E e)
		{
			return add(e);
		}

		public virtual E peek()
		{
			return queue.Peek();
		}

		public virtual E poll()
		{
			return queue.Dequeue();
		}

        public virtual E take() 
        {
            return queue.Dequeue();
        }

        public virtual void pollAll(IList<E> list) 
        {
            while (queue.Count > 0)
                list.Add(queue.Dequeue());    
        }

		public virtual int size()
		{
			return queue.Count;
		}

		public virtual bool isEmpty()
		{
			return size() == 0;
		}

		public virtual void clear()
		{
			queue.Clear();
		}

        public virtual int getCapacity()
        {
            return capacity;
        }

		public virtual void forEach(Action<E> consumer)
        {
			foreach (E item in queue)
            {
				consumer(item);
            }
        }
	}
}
