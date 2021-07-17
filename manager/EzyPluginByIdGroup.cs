using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.manager
{
	public interface EzyPluginByIdGroup
	{
		void addPlugin(EzyPlugin plugin);

        EzyPlugin removePlugin(int pluginId);

		EzyPlugin getPluginById(int pluginId);
	}
}
