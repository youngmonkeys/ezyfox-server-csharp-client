using com.tvd12.ezyfoxserver.client.evt;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public interface IEzyEventHandler
	{
	}

	public interface EzyEventHandler : IEzyEventHandler
	{
		void handle(EzyEvent evt);
	}
}
