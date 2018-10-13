using System.Threading;

namespace com.tvd12.ezyfoxserver.client.concurrent
{
	public class AtomicInteger
	{
		private int value = 0;

		public int get()
		{
			return this.value;
		}

		public void set(int val)
		{
			Interlocked.Exchange(ref value, val);
		}

		public int incrementAndGet()
		{
			int answer = Interlocked.Increment(ref value);
			return answer;
		}

		public int decrementAndGet()
		{
			int answer = Interlocked.Decrement(ref value);
			return answer;
		}
	}
}
