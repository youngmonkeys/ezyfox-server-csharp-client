using System;
using com.tvd12.ezyfoxserver.client.handler;

namespace com.tvd12.ezyfoxserver.client.setup
{
	public class EzySimpleAppSetup : EzyAppSetup
	{
        protected readonly EzyAppDataHandlers dataHandlers;
        protected readonly EzySetup parent;

		public EzySimpleAppSetup(EzyAppDataHandlers dataHandlers, EzySetup parent)
		{
			this.parent = parent;
			this.dataHandlers = dataHandlers;
		}

		public EzyAppSetup addDataHandler(Object cmd, EzyAppDataHandler dataHandler)
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
