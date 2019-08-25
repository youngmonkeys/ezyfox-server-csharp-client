using System.Threading;

namespace com.tvd12.ezyfoxserver.client.concurrent
{
	public class AtomicLong
	{
        protected long value = 0;

		public AtomicLong()
		{
		}

		public AtomicLong(long initValue)
		{
			this.value = initValue;
		}

		public long get()
		{
			return this.value;
		}

		public void set(long val)
		{
			Interlocked.Exchange(ref value, val);
		}

		public long incrementAndGet()
		{
			return Interlocked.Increment(ref value);
		}

		public long decrementAndGet()
		{
			return Interlocked.Decrement(ref value);
		}
	}
}
