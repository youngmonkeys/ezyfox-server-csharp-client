using System;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.request;

namespace com.tvd12.ezyfoxserver.client.entity
{
	public interface EzyPlugin
	{
		int getId();

		String getName();

		EzyClient getClient();

		EzyZone getZone();

        void send(EzyRequest request);

        void send(EzyRequest request, bool encrypted);

        void send(String cmd);

        void send(String cmd, bool encrypted);

        void send(String cmd, EzyData data);

        void send(String cmd, EzyData data, bool encrypted);

        void udpSend(EzyRequest request);

        void udpSend(EzyRequest request, bool encrypted);

        void udpSend(String cmd);

        void udpSend(String cmd, bool encrypted);

        void udpSend(String cmd, EzyData data);

        void udpSend(String cmd, EzyData data, bool encrypted);

        EzyPluginDataHandler getDataHandler(Object cmd);
	}
}
