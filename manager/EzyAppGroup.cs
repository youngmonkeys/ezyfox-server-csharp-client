using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.manager
{
	public interface EzyAppGroup : EzyAppByIdGroup
	{
		EzyApp getApp();

		IList<EzyApp> getAppList();

		EzyApp getAppByName(String appName);

	}

}
