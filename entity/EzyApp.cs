using System;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.socket;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.entity
{
	public interface EzyApp : EzySender, EzyInstanceFetcher, EzyDestroyable
	{
		int getId();

		String getName();

		EzyClient getClient();

		EzyZone getZone();

		EzyAppDataHandler getDataHandler(Object cmd);
	}
}
