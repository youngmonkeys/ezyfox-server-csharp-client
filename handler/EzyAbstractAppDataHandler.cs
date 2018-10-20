using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public abstract class EzyAbstractAppDataHandler<D> : 
        EzyLoggable,
        EzyAppDataHandler where D : EzyData
	{
		public void handle(EzyApp app, EzyData data)
		{
			process(app, (D)data);
		}

		protected abstract void process(EzyApp app, D data);
	}
}
