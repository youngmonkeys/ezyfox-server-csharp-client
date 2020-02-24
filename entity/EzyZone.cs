using System;
using com.tvd12.ezyfoxserver.client.manager;

namespace com.tvd12.ezyfoxserver.client.entity
{
    public interface EzyZone
    {
        int getId();

    	String getName();

    	EzyClient getClient();

    	EzyAppManager getAppManager();

        EzyApp getApp();
    }

}
