using System;
using System.Collections.Generic;

namespace com.tvd12.ezyfoxserver.client.util
{
    public class EzyValueStack<V>
    {
        protected V topValue;
        protected V lastValue;
        protected V defaultValue;
        protected readonly Stack<V> values;

        public EzyValueStack(V defValue)
        {
            values = new Stack<V>();
            topValue = defValue;
            lastValue = defValue;
            defaultValue = defValue;
        }

        public V top()
        {
            lock (this)
            {
                return topValue;
            }
        }

        public V last()
        {
            lock (this)
            {
                return lastValue;
            }
        }

        public V pop()
        {
            lock (this)
            {
                int size = values.Count;
                if (size > 0)
                {
                    topValue = values.Pop();
                }
                else
                {
                    topValue = defaultValue;
                }
                return topValue;
            }
        }

        public void popAll(IList<V> buffer)
        {
            lock (this)
            {
                while (values.Count > 0)
                    buffer.Add(values.Pop());
            }
        }


        public void push(V value)
        {
            lock (this)
            {
                topValue = value;
                lastValue = value;
                values.Push(value);
            }
        }

        public void clear()
        {
            lock (this)
            {
                topValue = defaultValue;
                lastValue = defaultValue;
                values.Clear();
            }
        }


        public int size()
        {
            lock (this)
            {
                int size = values.Count;
                return size;
            }
        }


    }
}
