using System;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.handler;

namespace com.tvd12.ezyfoxserver.client.manager
{
	public interface EzyHandlerManager
	{
		EzyDataHandler getDataHandler(Object cmd);

		EzyEventHandler<E> getEventHandler<E>(Object eventType) where E : EzyEvent;

		void addDataHandler(Object cmd, EzyDataHandler dataHandler);

		void addEventHandler(Object eventType, IEzyEventHandler eventHandler);

		EzyAppDataHandlers getAppDataHandlers(String appName);
	}
}
