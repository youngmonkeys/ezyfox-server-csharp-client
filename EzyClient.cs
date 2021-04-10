using System;
using com.tvd12.ezyfoxserver.client.setup;
using com.tvd12.ezyfoxserver.client.socket;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.manager;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.request;

namespace com.tvd12.ezyfoxserver.client
{

	public interface EzyClient
	{
        EzySetup setup();

		void connect(String host, int port);

		bool reconnect();

        void send(EzyRequest request);

        void send(EzyCommand cmd, EzyArray data);

		void disconnect(int reason = (int)EzyDisconnectReason.CLOSE);

		void processEvents();

        void udpConnect(int port);

        void udpConnect(String host, int port);

        void udpSend(EzyRequest request);

        void udpSend(EzyCommand cmd, EzyArray data);

        String getName();

		EzyClientConfig getConfig();

        EzyUser getMe();

		EzyZone getZone();

		EzyConnectionStatus getStatus();

		void setStatus(EzyConnectionStatus status);

        bool isConnected();

        EzyConnectionStatus getUdpStatus();

        void setUdpStatus(EzyConnectionStatus status);

        bool isUdpConnected();

        void setSessionId(long sessionId);

        void setSessionToken(String token);

        EzyISocketClient getSocket();

        EzyApp getApp();

        EzyApp getAppById(int appId);

		EzyPingManager getPingManager();

        EzyPingSchedule getPingSchedule();

		EzyHandlerManager getHandlerManager();

	}
}
