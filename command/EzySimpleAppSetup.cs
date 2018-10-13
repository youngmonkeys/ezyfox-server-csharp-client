using System;
using com.tvd12.ezyfoxserver.client.handler;

namespace com.tvd12.ezyfoxserver.client.command
{
	public class EzySimpleAppSetup : EzyAppSetup
	{
		private readonly EzyAppDataHandlers dataHandlers;
		private readonly EzySetup parent;

		public EzySimpleAppSetup(EzyAppDataHandlers dataHandlers, EzySetup parent)
		{
			this.parent = parent;
			this.dataHandlers = dataHandlers;
		}

		public EzyAppSetup addDataHandler(Object cmd, IEzyAppDataHandler dataHandler)
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
