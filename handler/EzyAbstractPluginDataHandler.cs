using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public abstract class EzyAbstractPluginDataHandler<D> : 
        EzyLoggable,
        EzyPluginDataHandler where D : EzyData
	{
		public void handle(EzyPlugin plugin, EzyData data)
		{
			process(plugin, (D)data);
		}

		protected abstract void process(EzyPlugin plugin, D data);
	}
}
