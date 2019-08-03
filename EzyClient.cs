using System;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.socket;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.manager;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.constant;

namespace com.tvd12.ezyfoxserver.client
{

	public interface EzyClient :
		EzySender,
		EzyAppByIdGroup, EzyInstanceFetcher
	{
		void connect(String host, int port);

		void connect();

		bool reconnect();

		void disconnect();

		void processEvents();

        String getName();

		EzyClientConfig getConfig();

		EzyZone getZone();

		String getZoneName();

		EzyUser getMe();

		EzyConnectionStatus getStatus();

		void setStatus(EzyConnectionStatus status);

		EzyPingManager getPingManager();

		EzyHandlerManager getHandlerManager();

	}
}
