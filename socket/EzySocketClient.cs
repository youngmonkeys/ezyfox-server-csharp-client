using System;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public interface EzySocketClient : EzySender
	{
    	void connect(String host, int port);

    	void connect();

		bool reconnect();

    	void disconnect();

	}

}
