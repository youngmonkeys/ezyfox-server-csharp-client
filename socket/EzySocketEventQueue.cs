using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.evt;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzySocketEventQueue
    {
        protected readonly Queue<EzyEvent> events;

        public EzySocketEventQueue()
        {
            this.events = new Queue<EzyEvent>();
        }

        public void addEvent(EzyEvent evt)
        {
            lock (events)
            {
                events.Enqueue(evt);
            }
        }

        public void popAll(IList<EzyEvent> buffer)
        {
            lock (events)
            {
                while (events.Count > 0)
                    buffer.Add(events.Dequeue());
            }
        }

        public void clear() 
        {
            lock(events) 
            {
                events.Clear();
            }
        }
	}
}
