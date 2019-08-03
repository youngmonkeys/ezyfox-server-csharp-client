using System;
using System.Threading;
using com.tvd12.ezyfoxserver.client.util;
using static com.tvd12.ezyfoxserver.client.io.EzyDateTimes;

namespace com.tvd12.ezyfoxserver.client.concurrent
{
    public class EzyScheduleAtFixedRate : EzyStoppable
    {
        protected volatile bool active;
        protected volatile bool started;
        protected ThreadList threadList;
        protected readonly String threadName;
        protected readonly int threadPoolSize;

        public EzyScheduleAtFixedRate(String threadName)
        {
            this.active = false;
            this.started = false;
            this.threadPoolSize = 1;
            this.threadName = threadName;
        }

        public void schedule(ThreadStart task, int delay, int period) {
            if (started) 
                return;
            this.started = true;
            this.threadList = new ThreadList(threadPoolSize, threadName, () =>
            {
                if (delay > 0)
                    Thread.Sleep(delay);
                this.active = true;
                while(active) {
                    DateTime startTime = DateTime.Now;
                    task.Invoke();
                    int elapsedTime = (int)getOffsetMillis(startTime, DateTime.Now);
                    int remainSleepTime = period - elapsedTime;
                    if (remainSleepTime > 0)
                        Thread.Sleep(remainSleepTime);
                }
            });
            this.threadList.start();
        }

        public void stop() {
            this.active = false;
        }
    }
}
