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
        protected readonly Object sleepLock;

        public EzyScheduleAtFixedRate(String threadName)
        {
            this.active = false;
            this.started = false;
            this.threadPoolSize = 1;
            this.threadName = threadName;
            this.sleepLock = new Object();
        }

        public void schedule(ThreadStart task, int delay, int period) {
            lock (this)
            {
                if (started)
                    return;
                this.started = true;
                this.threadList = new ThreadList(threadPoolSize,
                                                 threadName,
                                                 () => startLoop(task, delay, period));
                this.threadList.start();
            }
        }

        public void startLoop(ThreadStart task, int delay, int period) {
            if (delay > 0)
                sleep(delay);
            this.active = true;
            while (active)
            {
                if (stoppable())
                    break;
                DateTime startTime = DateTime.Now;
                task.Invoke();
                int elapsedTime = (int)getOffsetMillis(startTime, DateTime.Now);
                int remainSleepTime = period - elapsedTime;
                if (remainSleepTime > 0)
                    sleep(remainSleepTime);
            }
        }

        public void stop() {
            lock (this)
            {
                this.active = false;
                this.wakeup();
            }
        }

        protected bool stoppable() {
            lock(this) {
                return active == false;
            }
        }

        protected void sleep(int time) {
            lock(sleepLock) {
                if(active)
                    Monitor.Wait(sleepLock, time);
            }
        }

        protected void wakeup() {
            lock(sleepLock) {
                Monitor.Pulse(sleepLock);
            }
        }
    }
}
