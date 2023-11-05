using System;
using System.Threading;
using com.tvd12.ezyfoxserver.client.concurrent;
using com.tvd12.ezyfoxserver.client.statistics;
using com.tvd12.ezyfoxserver.client.util;
namespace com.tvd12.ezyfoxserver.client.socket
{
    public abstract class EzySocketAdapter : EzyLoggable, EzyEventLoopEvent
    {
        protected volatile bool active;
        protected volatile bool stopped;
        protected readonly Object adapterLock;
        protected EzyStatistics networkStatistics;
        protected EzyEventLoopGroup eventLoopGroup;

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
                {
                    return;
                }
                active = true;
                stopped = false;
                if (eventLoopGroup != null)
                {
                    eventLoopGroup.addEvent(this);
                }
                else
                {
                    Thread newThread = new Thread(run);
                    newThread.Name = getThreadName();
                    newThread.Start();
                }
            }    
        }

        public virtual bool call()
        {
            return false;
        }

        public void onFinished()
        {
            setStopped(true);
        }

        public void onRemoved()
        {
            // do nothing
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
                if (eventLoopGroup != null)
                {
                    eventLoopGroup.removeEvent(this);
                }
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

        protected void addSocketReadStats(int readBytes)
        {
            EzyNetworkStats networkStats = networkStatistics
                .getSocketStats()
                .getNetworkStats();
            networkStats.addReadBytes(readBytes);
            networkStats.addReadPackets(1);
        }

        protected void addSocketWriteStats(int writtenBytes)
        {
            EzyNetworkStats networkStats = networkStatistics
                .getSocketStats()
                .getNetworkStats();
            networkStats.addWrittenPackets(1);
            networkStats.addWrittenBytes(writtenBytes);
        }

        public void setNetworkStatistics(EzyStatistics networkStatistics)
        {
            this.networkStatistics = networkStatistics;
        }

        public void setEventLoopGroup(EzyEventLoopGroup eventLoopGroup)
        {
            this.eventLoopGroup = eventLoopGroup;
        }
    }
}
