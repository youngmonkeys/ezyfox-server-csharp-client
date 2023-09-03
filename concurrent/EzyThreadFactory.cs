using System;
using System.Threading;
using com.tvd12.ezyfoxserver.client.builder;

namespace com.tvd12.ezyfoxserver.client.concurrent
{
    public class EzyThreadFactory
    {
        protected String poolName;
        protected String threadPrefix;
        protected AtomicInteger threadCounter = new AtomicInteger();

        private static readonly AtomicInteger POOL_COUNTER = new AtomicInteger();

        protected EzyThreadFactory(Builder builder)
        {
            int poolId = POOL_COUNTER.incrementAndGet();
            this.poolName = builder._poolName;
            this.threadPrefix = poolName + '-' + poolId + '-';
        }

        public Thread newThread(ThreadStart runnable)
        {
            Thread thread = new Thread(runnable);
            thread.Name = getThreadName();
            return thread;
        }

        protected String getThreadName()
        {
            return threadPrefix + "-" + threadCounter.incrementAndGet();
        }

        public static Builder builder()
        {
            return new Builder();
        }


        public class Builder : EzyBuilder<EzyThreadFactory>
        {
            public String _poolName;

            public Builder()
            {
                this._poolName = "";
            }


            public Builder poolName(String poolName)
            {
                this._poolName = poolName;
                return this;
            }

            public EzyThreadFactory build()
            {
                return new EzyThreadFactory(this);
            }
        }
    }

}
