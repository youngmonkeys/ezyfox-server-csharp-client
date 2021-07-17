using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.manager
{
	public interface EzyPluginGroup : EzyPluginByIdGroup
	{
		EzyPlugin getPlugin();

		IList<EzyPlugin> getPluginList();

		EzyPlugin getPluginByName(String pluginName);

	}

}
