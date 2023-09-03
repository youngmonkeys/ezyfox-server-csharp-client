using System;
using com.tvd12.ezyfoxserver.client.function;

namespace com.tvd12.ezyfoxserver.client.util
{
    public class EzyRoundRobin<T>
    {

        protected readonly EzyQueue<T> queue;

        public EzyRoundRobin()
        {
            this.queue = new EzyQueue<T>();
        }

        public EzyRoundRobin(Func<T> supplier, int size) : this()
        {
            for (int i = 0; i < size; ++i)
            {
                this.queue.offer(supplier());
            }
        }

        public void add(T item)
        {
            lock(queue) {
                this.queue.add(item);
            }
        }

        public T get()
        {
            T last = default(T);
            lock(queue) {
                last = queue.poll();
                queue.offer(last);
            }
            return last;
        }

        public void forEach(Action<T> consumer)
        {
            lock(queue) {
                queue.forEach(consumer);
            }
        }
    }
}
