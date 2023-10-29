using System;
using System.Text;
using System.Threading;

namespace com.tvd12.ezyfoxserver.client.concurrent
{
    public interface EzyFuture<T>
    {
        void setResult(T result);

        T get();

        T get(TimeSpan timeout);

        void cancel();

        bool isDone();

        bool isCancelled();
    }

    public class EzyFutureTask<T> : EzyFuture<T>
    {
        protected T result;
        protected volatile bool done;
        protected volatile bool cancelled;
        protected readonly long id = ID_GENTOR.incrementAndGet();
        protected readonly Object synchronizedLock = new Object();
        protected readonly AutoResetEvent state = new AutoResetEvent(false);

        private static readonly AtomicLong ID_GENTOR = new AtomicLong(0);


        public T get()
        {
            return get(TimeSpan.Zero);
        }

        public T get(TimeSpan timeout)
        {
            while (!done && !cancelled)
            {
                if (timeout.Equals(TimeSpan.Zero))
                {
                    bool success = state.WaitOne();
                    if (!success)
                        throw new ThreadInterruptedException(
                            "Task: " + id + " has interrupted"
                        );
                }
                else
                {
                    bool success = state.WaitOne(timeout);
                    if (!success)
                    {
                        throw new TimeoutException(getTimeoutMessage());
                    }
                }
            }
            if (cancelled)
            {
                throw new OperationCanceledException(
                    "Task: " + id + " has canceled"
                );
            }
            return result;
        }

        public void setResult(T result)
        {
            lock (synchronizedLock)
            {
                if (!done && !cancelled)
                {
                    this.result = result;
                    this.done = true;
                    this.state.Set();
                }
            }
        }

        public void cancel()
        {
            lock (synchronizedLock)
            {
                if (!done)
                {
                    cancelled = true;
                    state.Set();
                }
            }
        }

        public bool isDone()
        {
            return done;
        }

        public bool isCancelled()
        {
            return cancelled;
        }

        private String getTimeoutMessage()
        {
            return new StringBuilder()
                .Append("Task: ").Append(id)
                .ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is EzyFutureTask<T>
                && ((EzyFutureTask<T>)obj).id == id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public override string ToString()
        {
            return new StringBuilder()
                .Append("Task[Id: ").Append(id)
                .Append("]")
                .ToString();
        }
    }
}
