using System;
namespace com.tvd12.ezyfoxserver.client.manager
{
	public interface EzyPingManager
	{
		int getPingPeriod();

		int increaseLostPingCount();

		int getMaxLostPingCount();

		void setLostPingCount(int count);
	}
}
