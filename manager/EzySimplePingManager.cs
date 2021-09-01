using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.concurrent;

namespace com.tvd12.ezyfoxserver.client.manager
{
	public class EzySimplePingManager : EzyPingManager
	{
        protected int pingPeriod;
        protected int maxLostPingCount;
        protected readonly AtomicInteger lostPingCount;

		public EzySimplePingManager(EzyPingConfig config)
		{
			this.pingPeriod = config.getPingPeriod();
			this.maxLostPingCount = config.getMaxLostPingCount();
			this.lostPingCount = new AtomicInteger();
		}

		public int getPingPeriod()
		{
			return pingPeriod;
		}

		public void setLostPingCount(int count)
		{
			lostPingCount.set(count);
		}

		public int increaseLostPingCount()
		{
			return lostPingCount.incrementAndGet();
		}

		public int getMaxLostPingCount()
		{
			return maxLostPingCount;
		}

	}
}
