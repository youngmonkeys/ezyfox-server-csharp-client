using System;
using System.Threading;

using com.tvd12.ezyfoxserver.client.statistics;
using com.tvd12.ezyfoxserver.client.util;
namespace com.tvd12.ezyfoxserver.client.socket
{
    public abstract class EzySocketAdapter : EzyLoggable
    {
        protected volatile bool active;
        protected volatile bool stopped;
        protected readonly Object adapterLock;
        protected EzyStatistics networkStatistics;

        public EzySocketAdapter()
        {
            this.active = false;
            this.stopped = false;
            this.adapterLock = new Object();
        }

        public void start() 
        {
            lock(adapterLock) 
            {
                if (active) 
                    return;
                active = true;
                stopped = false;
                Thread newThread = new Thread(run);
                newThread.Name = getThreadName();
                newThread.Start();
            }    
        }

        protected abstract String getThreadName();

        protected virtual void run() 
        {
            update();
            setStopped(true);
        }

        protected abstract void update();

        public void stop() 
        {
            lock(adapterLock)
            {
                clear();
                active = false;
            }
        }

        protected virtual void clear() 
        {
        }

        protected void setActive(bool active)
        {
            lock(adapterLock)
            {
                this.active = active;
            }
        }

        protected void setStopped(bool stopped)
        {
            lock(adapterLock)
            {
                this.stopped = stopped;
            }
        }

        public bool isActive() 
        {
            lock (adapterLock)
            {
                return active;
            }
        }

        public bool isStopped()
        {
            lock (adapterLock)
            {
                return stopped;
            }
        }

        public void setNetworkStatistics(EzyStatistics networkStatistics)
        {
            this.networkStatistics = networkStatistics;
        }
    }
}
