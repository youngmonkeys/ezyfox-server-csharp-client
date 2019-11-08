using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.manager
{
	public interface EzyAppByIdGroup
	{
		void addApp(EzyApp app);

        EzyApp removeApp(int appId);

		EzyApp getAppById(int appId);
	}
}
