using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public abstract class EzyAbstractSocketClient : EzyLoggable, EzySocketClient
	{
		public abstract void connect();
		public abstract void connect(string host, int port);
		public abstract void disconnect();
		public abstract bool reconnect();
		public abstract void send(EzyRequest request);
		public abstract void send(object cmd, EzyData data);
		protected abstract void connect0();
		protected abstract void resetComponents();
	}

}
