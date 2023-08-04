using System;
using com.tvd12.ezyfoxserver.client.setup;
using com.tvd12.ezyfoxserver.client.socket;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.manager;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.statistics;

namespace com.tvd12.ezyfoxserver.client
{

	public interface EzyClient
	{
        EzySetup setup();

        void connect(String url);

        void connect(String host, int port);

		bool reconnect();

        void send(EzyRequest request, bool encrypted = false);

        void send(EzyCommand cmd, EzyArray data, bool encrypted = false);

        void disconnect(int reason = (int)EzyDisconnectReason.CLOSE);

        void close();

		void processEvents();

        void udpConnect(int port);

        void udpConnect(String host, int port);

        void udpSend(EzyRequest request, bool encrypted = false);

        void udpSend(EzyCommand cmd, EzyArray data, bool encrypted = false);

        String getName();

		EzyClientConfig getConfig();

        bool isEnableSSL();

        bool isEnableDebug();

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

        void setSessionKey(byte[] sessionKey);

        byte[] getSessionKey();

        void setPrivateKey(byte[] privateKey);

        byte[] getPrivateKey();

        void setPublicKey(byte[] publicKey);

        byte[] getPublicKey();

        EzyISocketClient getSocket();

        EzyApp getApp();

        EzyApp getAppById(int appId);

        EzyPlugin getPlugin();

        EzyPlugin getPluginById(int pluginId);

        EzyPingManager getPingManager();

        EzyPingSchedule getPingSchedule();

		EzyHandlerManager getHandlerManager();

        EzyStatistics getNetworkStatistics();

        EzyTransportType getTransportType();
    }
}
