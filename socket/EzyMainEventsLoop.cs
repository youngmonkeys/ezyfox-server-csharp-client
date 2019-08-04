using System;
using System.Threading;
using System.Collections.Generic;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzyMainEventsLoop
    {
        protected volatile bool active;
        protected readonly EzyClients clients;
        protected readonly IList<EzyClient> cachedClients;

        public EzyMainEventsLoop() {
            this.clients = EzyClients.getInstance();
            this.cachedClients = new List<EzyClient>();
        }

        public void start()
        {
            start(3);
        }

        public void start(int sleepTime) 
        {
            this.active = true;
            while (active)
            {
                Thread.Sleep(sleepTime);
                clients.getClients(cachedClients);
                foreach (EzyClient one in cachedClients)
                    one.processEvents();
            }
        }

        public void stop() 
        {
            this.active = false;
        }
    }
}
