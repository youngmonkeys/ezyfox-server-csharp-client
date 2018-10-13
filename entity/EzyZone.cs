using System;
using com.tvd12.ezyfoxserver.client;
using com.tvd12.ezyfoxserver.client.manager;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.entity
{
public interface EzyZone : EzyDestroyable
{

    int getId();

	String getName();

	EzyClient getClient();

	EzyAppManager getAppManager();

}

}
