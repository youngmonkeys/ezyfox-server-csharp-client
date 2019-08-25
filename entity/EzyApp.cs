using System;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.entity
{
	public interface EzyApp
	{
		int getId();
		String getName();
		EzyClient getClient();
		EzyZone getZone();
        void send(EzyRequest request);
        void send(String cmd, EzyData data);
		EzyAppDataHandler getDataHandler(Object cmd);
	}
}
