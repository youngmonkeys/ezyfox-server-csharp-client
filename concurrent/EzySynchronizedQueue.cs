using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.concurrent
{
    public class EzySynchronizedQueue<E> : EzyQueue<E>
    {
        public EzySynchronizedQueue() : base()
        {
        }

        public EzySynchronizedQueue(int capacity) : base(capacity)
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
            lock (queue)
            {
                E e = queue.Peek();
                return e;
            }
        }

        public override E poll()
        {
            lock (queue)
            {
                E e = queue.Dequeue();
                return e;
            }
        }

        public override E take()
        {
            lock (queue)
            {
                E e = queue.Dequeue();
                return e;
            }
        }

        public override void pollAll(IList<E> list)
        {
            lock (queue)
            {
                while (queue.Count > 0)
                    list.Add(queue.Dequeue());
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
    }
}
