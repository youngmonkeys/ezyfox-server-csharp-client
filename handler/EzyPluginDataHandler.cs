using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public interface IEzyPluginDataHandler
	{
	}

	public interface EzyPluginDataHandler : IEzyPluginDataHandler
	{
		void handle(EzyPlugin plugin, EzyData data);
	}
}
