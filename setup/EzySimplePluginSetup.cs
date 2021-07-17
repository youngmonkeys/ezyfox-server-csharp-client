using System;
using com.tvd12.ezyfoxserver.client.handler;

namespace com.tvd12.ezyfoxserver.client.setup
{
	public class EzySimplePluginSetup : EzyPluginSetup
	{
        protected readonly EzyPluginDataHandlers dataHandlers;
        protected readonly EzySetup parent;

		public EzySimplePluginSetup(EzyPluginDataHandlers dataHandlers, EzySetup parent)
		{
			this.parent = parent;
			this.dataHandlers = dataHandlers;
		}

		public EzyPluginSetup addDataHandler(Object cmd, EzyPluginDataHandler dataHandler)
		{
			dataHandlers.addHandler(cmd, dataHandler);
			return this;
		}

		public EzySetup done()
		{
			return parent;
		}
	}
}
