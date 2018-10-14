using System;
using com.tvd12.ezyfoxserver.client.concurrent;

namespace com.tvd12.ezyfoxserver.client.manager
{
	public class EzySimplePingManager : EzyPingManager
	{
		private int pingPeriod;
		private int maxLostPingCount;
		private readonly AtomicInteger lostPingCount;

		public EzySimplePingManager()
		{
			this.pingPeriod = 5000;
			this.maxLostPingCount = 5;
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
