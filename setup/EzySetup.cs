using System;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.handler;

namespace com.tvd12.ezyfoxserver.client.setup
{
	public interface EzySetup
	{
		EzySetup addDataHandler(Object cmd, EzyDataHandler dataHandler);

		EzySetup addEventHandler(EzyEventType eventType, EzyEventHandler eventHandler);

		EzyAppSetup setupApp(String appName);

	}

}
