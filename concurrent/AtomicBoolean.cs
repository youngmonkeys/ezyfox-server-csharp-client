using System.Threading;

namespace com.tvd12.ezyfoxserver.client.concurrent
{
    public class AtomicBoolean
    {
        protected int value;

        public AtomicBoolean() : this(false)
        {
        }

        public AtomicBoolean(bool initValue)
        {
            value = initValue ? 1 : 0;
        }

        public bool get()
        {
            return value > 0;
        }

        public void set(bool val)
        {
            int intValue = val ? 1 : 0;
            Interlocked.Exchange(ref value, intValue);
        }

        public bool compareAndSet(bool expectedValue, bool newValue)
        {
            int expectedIntValue = expectedValue ? 1 : 0;
            int intValue = newValue ? 1 : 0;
            int originalValue = Interlocked.CompareExchange(
                ref value,
                intValue,
                expectedIntValue
            );
            return value != originalValue;
        }
    }
}
