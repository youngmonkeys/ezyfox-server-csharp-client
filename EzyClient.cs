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
		void disconnect(int reason);
		void processEvents();
        String getName();
		EzyClientConfig getConfig();
        EzyUser getMe();
		EzyZone getZone();
		EzyConnectionStatus getStatus();
		void setStatus(EzyConnectionStatus status);
        EzyApp getAppById(int appId);
		EzyPingManager getPingManager();
        EzyPingSchedule getPingSchedule();
		EzyHandlerManager getHandlerManager();

	}
}
