using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.concurrent
{
    public class EzyEventLoopGroup : EzyLoggable
    {
        private readonly EzyRoundRobin<EventLoop> eventLoops;
        private readonly IDictionary<EzyEventLoopEvent, EventLoop> eventLoopByEvent;

        public static readonly int DEFAULT_MAX_SLEEP_TIME = 3;

        public EzyEventLoopGroup(int numberOfThreads) : this(
                numberOfThreads,
                EzyThreadFactory.builder()
                    .poolName("event-loop")
                    .build()
            )
        {
        }

        public EzyEventLoopGroup(
            int numberOfThreads,
            EzyThreadFactory threadFactory
        ) : this(
                DEFAULT_MAX_SLEEP_TIME,
                numberOfThreads,
                threadFactory
            )
        {
        }

        public EzyEventLoopGroup(
            int maxSleepTime,
            int numberOfThreads,
            EzyThreadFactory threadFactory
        )
        {
            eventLoopByEvent = new ConcurrentDictionary<EzyEventLoopEvent, EventLoop>();
            int index = 0;
            eventLoops = new EzyRoundRobin<EventLoop>(
                () =>
                {
                    return new EventLoop(
                        index++,
                        maxSleepTime,
                        threadFactory
                    );
                },
                numberOfThreads
            );
            for (int i = 0; i < numberOfThreads; ++i)
            {
                eventLoops.get().start();
            }
        }

        public void addEvent(EzyEventLoopEvent evt)
        {
            EventLoop eventLoop = eventLoops.get();
            eventLoopByEvent.Add(
                evt is ScheduledEvent
                    ? ((ScheduledEvent)evt).runEvent
                    : evt,
                eventLoop
            );
            eventLoop.addEvent(evt);
        }

        public void addScheduleEvent(
            EzyEventLoopEvent evt,
            long period
        )
        {
            addScheduleEvent(evt, 0, period);
        }

        public void addScheduleEvent(
            EzyEventLoopEvent evt,
            long delayTime,
            long period
        )
        {
            addEvent(new ScheduledEvent(evt, delayTime, period));
        }

        public void addOneTimeEvent(
            Action task,
            long delayTime
        )
        {
            EzyEventLoopEvent wrapper = new EzyEventLoopOneTimeEvent(
                task,
                eventLoopByEvent
            );
            addEvent(
                new ScheduledEvent(
                    wrapper,
                    delayTime,
                    delayTime
                )
        );
        }

        public void removeEvent(EzyEventLoopEvent evt)
        {
            if (eventLoopByEvent.ContainsKey(evt))
            {
                EventLoop eventLoop = eventLoopByEvent[evt];
                eventLoop.removeEvent(evt);

            }
        }

        public void shutdown()
        {
            eventLoops.forEach((eventLoop) =>
            {
                eventLoop.shutdownAndGet();
            });
        }

        public List<EzyEventLoopEvent> shutdownAndGet()
        {
            List<EzyEventLoopEvent> unfinishedEvents = new List<EzyEventLoopEvent>();
            eventLoops.forEach(eventLoop =>
            {
                unfinishedEvents.AddRange(eventLoop.shutdownAndGet());
            });
            return unfinishedEvents;
        }

        public String toString()
        {
            StringBuilder builder = new StringBuilder()
                .Append("EzyEventLoopGroup{\n  numberOfEvents=")
                .Append(eventLoopByEvent.Count)
                .Append(",\n  eventLoops=[");
            eventLoops.forEach(it =>
                builder.Append("\n    ").Append(it)
            );
            return builder.Append("\n]}").ToString();
        }

        public class EventLoop : EzyLoggable
        {
            private readonly int index;
            private readonly int maxSleepTime;
            private readonly AtomicBoolean active;
            private readonly AtomicBoolean stopped;
            private readonly EzyFuture<Boolean> shutdownFuture;
            private readonly EzyThreadFactory threadFactory;
            private readonly List<EzyEventLoopEvent> removeEvents;
            private readonly IDictionary<EzyEventLoopEvent, EzyEventLoopEvent> events;

            public EventLoop(
                int index,
                int maxSleepTime,
                EzyThreadFactory threadFactory
            )
            {
                this.index = index;
                this.maxSleepTime = maxSleepTime;
                this.threadFactory = threadFactory;
                this.active = new AtomicBoolean();
                this.stopped = new AtomicBoolean();
                this.events = new Dictionary<EzyEventLoopEvent, EzyEventLoopEvent>();
                this.removeEvents = new List<EzyEventLoopEvent>();
                this.shutdownFuture = new EzyFutureTask<bool>();
            }

            public void addEvent(EzyEventLoopEvent evt)
            {
                if (!active.get())
                {
                    throw new Exception("event loop has stopped");
                }
                events.Add(
                    evt is ScheduledEvent
                        ? ((ScheduledEvent)evt).runEvent
                        : evt,
                    evt
                );
            }

            public void removeEvent(EzyEventLoopEvent evt)
            {
                lock (removeEvents)
                {
                    removeEvents.Add(evt);
                }
            }

            private void doRemoveEvent(EzyEventLoopEvent evt)
            {
                events.Remove(
                    evt is ScheduledEvent
                        ? ((ScheduledEvent)evt).runEvent
                        : evt
                );
                try
                {
                    evt.onRemoved();
                }
                catch (Exception e)
                {
                    logger.info("remove event: " + evt + " error", e);
                }
            }

            public void start()
            {
                threadFactory.newThread(() => doStart())
                    .Start();
            }

            private void doStart()
            {
                active.set(true);
                List<EzyEventLoopEvent> eventBuffers = new List<EzyEventLoopEvent>();
                while (active.get())
                {
                    DateTime startTime = DateTime.Now;
                    eventBuffers.AddRange(events.Values);
                    foreach (EzyEventLoopEvent evt in eventBuffers)
                    {
                        try
                        {
                            if (evt is ScheduledEvent)
                            {
                                ScheduledEvent scheduledEvent = (ScheduledEvent)evt;
                                if (scheduledEvent.isNotFireTime())
                                {
                                    continue;
                                }
                            }
                            if (!evt.call())
                            {
                                lock (removeEvents)
                                {
                                    removeEvents.Add(evt);
                                }
                                evt.onFinished();
                            }
                        }
                        catch (Exception e)
                        {
                            logger.error("fatal error on event loop with event: {}", evt, e);
                        }
                    }
                    eventBuffers.Clear();
                    lock (removeEvents)
                    {
                        foreach (EzyEventLoopEvent evt in removeEvents)
                        {
                            doRemoveEvent(evt);
                        }
                        removeEvents.Clear();
                    }
                    int elapsedTime = (int)(DateTime.Now - startTime).TotalMilliseconds;
                    int sleepTime = maxSleepTime - elapsedTime;
                    if (sleepTime > 0)
                    {
                        try
                        {
                            //noinspection BusyWait
                            Thread.Sleep(sleepTime);
                        }
                        catch (Exception e)
                        {
                            logger.debug("event loop stopped", e);
                            break;
                        }
                    }
                }
                lock (this)
                {
                    stopped.set(true);
                    shutdownFuture.setResult(true);
                }
            }

            public List<EzyEventLoopEvent> shutdownAndGet()
            {
                active.set(false);
                lock (this)
                {
                    if (stopped.get())
                    {
                        shutdownFuture.setResult(true);
                    }
                }
                try
                {
                    shutdownFuture.get();
                }
                catch (Exception e)
                {
                    logger.debug("stop event eveloop group error", e);
                }
                return new List<EzyEventLoopEvent>(events.Values);
            }

            public String toString()
            {
                return "EventLoop-" + index + "{" +
                    "numberOfEvents=" + events.Count + ", " +
                    "numberOfRemoveEvents=" + removeEvents.Count +
                    '}';
            }
        }

        public class EzyEventLoopOneTimeEvent : EzyLoggable, EzyEventLoopEvent
        {
            private readonly Action task;
            private readonly IDictionary<EzyEventLoopEvent, EventLoop> eventLoopByEvent;

            public EzyEventLoopOneTimeEvent(
                Action task,
                IDictionary<EzyEventLoopEvent, EventLoop> eventLoopByEvent
            )
            {
                this.task = task;
                this.eventLoopByEvent = eventLoopByEvent;
            }

            public bool call()
            {
                try
                {
                    task();
                }
                catch (Exception e)
                {
                    logger.warn("call one time evt error", e);
                }
                return false;
            }

            public void onFinished()
            {
                eventLoopByEvent.Remove(this);
            }
        }

        public class ScheduledEvent : EzyEventLoopEvent
        {
            private readonly long period;
            public readonly EzyEventLoopEvent runEvent;
            private DateTime nextFireTime = new DateTime();

            public ScheduledEvent(
                EzyEventLoopEvent runEvent,
                long delayTime,
                long period
            )
            {
                this.period = period;
                this.runEvent = runEvent;
                this.nextFireTime = DateTime.Now.AddMilliseconds(
                    delayTime <= 0 ? 0 : period
                );
            }

            public bool isNotFireTime()
            {
                return DateTime.Now < nextFireTime;
            }

            public bool call()
            {
                nextFireTime = nextFireTime.AddMilliseconds(period);
                return runEvent.call();
            }

            public void onFinished()
            {
                runEvent.onFinished();
            }

            public void onRemoved()
            {
                runEvent.onRemoved();
            }
        }
    }
}
