using System;
using com.tvd12.ezyfoxserver.client.handler;

namespace com.tvd12.ezyfoxserver.client.setup
{
	public interface EzyPluginSetup
	{
		EzyPluginSetup addDataHandler(Object cmd, EzyPluginDataHandler dataHandler);

		EzySetup done();

	}
}
