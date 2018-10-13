using com.tvd12.ezyfoxserver.client.constant;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public interface EzyDisconnectionDelegate
	{
		void onDisconnected(EzyDisconnectReason reason);
	}

}
