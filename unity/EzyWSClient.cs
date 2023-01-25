using System;
using System.Collections.Generic;
using AOT;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.manager;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.setup;
using com.tvd12.ezyfoxserver.client.socket;
using com.tvd12.ezyfoxserver.client.statistics;
using com.tvd12.ezyfoxserver.client.util;
using Newtonsoft.Json;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public class EzyWSClient : EzyClient, EzyMeAware, EzyZoneAware
	{
		private static readonly EzyLogger LOGGER = EzyLoggerFactory.getLogger(typeof(EzyWSClient));

		private EzyUser me;
		private EzyZone zone;
		private long sessionId;
		private String sessionToken;
		private EzyClientConfig config;
		private EzyConnectionStatus status;

		private readonly EzySetup settingUp;
		private readonly EzyHandlerManager handlerManager;
		private readonly EzyPingManager pingManager;
		private readonly EzyPingSchedule pingSchedule;

		public EzyWSClient(EzyClientConfig config)
		{
			this.config = config;
			this.status = EzyConnectionStatus.NULL;
			this.pingManager = new EzySimplePingManager(config.getPing());
			this.pingSchedule = new EzyPingSchedule(this);
			this.handlerManager = new EzySimpleHandlerManager(this);
			this.settingUp = new EzySimpleSetup(handlerManager);
		}

		public void init()
		{
			String configJson = JsonConvert.SerializeObject(config);
			EzyWSProxy.setEventHandlerCallback(eventHandlerCallback);
			EzyWSProxy.setDataHandlerCallback(dataHandlerCallback);
			EzyWSProxy.run4(config.getClientName(), "init", configJson, initCallback);
		}

		[MonoPInvokeCallback(typeof(EzyDelegates.Delegate2))]
		public static void initCallback(String clientName, String jsonData)
		{
			LOGGER.debug("initCallback: clientName = " + clientName + ", jsonData = " + jsonData);
		}

		[MonoPInvokeCallback(typeof(EzyDelegates.EventHandlerDelegate))]
		public static void eventHandlerCallback(String clientName, String eventType, String jsonData)
		{
			LOGGER.debug("eventHandlerCallback: clientName = " + clientName + ", eventType = " + eventType + ", jsonData = " + jsonData);
			var eventJsDataDeserializer = EzyEventWSDataDeserializer.getInstance();
			var ezyEvent = eventJsDataDeserializer.deserializeEvent(eventType, jsonData);
			EzyClients.getInstance()
				.getClient(clientName)
				.getHandlerManager()
				.getEventHandler(ezyEvent.getType())
				.handle(ezyEvent);
		}

		[MonoPInvokeCallback(typeof(EzyDelegates.EventHandlerDelegate))]
		public static void dataHandlerCallback(String clientName, int commandId, String jsonData)
		{
			LOGGER.debug("dataHandlerCallback: clientName = " + clientName + ", commandId = " + commandId + ", jsonData = " + jsonData);
			var ezyData = EzyJsons.deserialize(jsonData);
			LOGGER.debug("den day roi 1");
			LOGGER.debug(ezyData.ToString());
			var command = (EzyCommand)commandId;
			LOGGER.debug("den day roi 2 " + command);
			LOGGER.debug(EzyClients.getInstance()
				             .getClient(clientName)
				             .getHandlerManager()
				             .getDataHandler(command) +
			             "");
			EzyClients.getInstance()
				.getClient(clientName)
				.getHandlerManager()
				.getDataHandler(command)
				.handle((EzyArray)ezyData);
		}

		public EzySetup setup()
		{
			return this.settingUp;
		}

		public void connect(string url)
		{
			var jsonData = JsonConvert.SerializeObject(
				new Dictionary<String, Object> { { "url", url } }
			);
			EzyWSProxy.run4(config.getClientName(), "connect", jsonData, null);
		}

		public void connect(string host, int port)
		{
			throw new NotImplementedException();
		}

		public bool reconnect()
		{
			EzyWSProxy.run3(config.getClientName(), "reconnect", reconnectCallback);
			return true;
		}

		[MonoPInvokeCallback(typeof(EzyDelegates.Delegate2))]
		public static void reconnectCallback(String clientName, String jsonData)
		{
			LOGGER.debug("reconnectCallback: clientName = " + clientName + ", jsonData = " + jsonData);
		}

		public void send(EzyRequest request)
		{
			LOGGER.debug("send request " + request);
			Object cmd = request.getCommand();
			EzyData data = request.serialize();
			send((EzyCommand)cmd, (EzyArray)data);
		}

		public void send(EzyCommand cmd, EzyArray data)
		{
			EzyObject obj = EzyEntityFactory.newObjectBuilder()
				.append("cmdId", cmd)
				.append("data", data)
				.build();
			String jsonData = EzyJsons.serialize(obj);
			EzyWSProxy.run4(config.getClientName(), "send", jsonData, sendCallback);
		}

		[MonoPInvokeCallback(typeof(EzyDelegates.Delegate2))]
		public static void sendCallback(String clientName, String jsonData)
		{
			LOGGER.debug("sendCallback: clientName = " + clientName + ", jsonData = " + jsonData);
		}

		public void disconnect(int reason)
		{
			EzyObject obj = EzyEntityFactory.newObjectBuilder()
				.append("reason", reason)
				.build();
			String jsonData = EzyJsons.serialize(obj);
			EzyWSProxy.run4(config.getClientName(), "disconnect", jsonData, null);
		}
		
		public void processEvents()
		{
			throw new NotImplementedException();
		}

		public void udpConnect(int port)
		{
			throw new NotImplementedException();
		}

		public void udpConnect(string host, int port)
		{
			throw new NotImplementedException();
		}

		public void udpSend(EzyRequest request)
		{
			throw new NotImplementedException();
		}

		public void udpSend(EzyCommand cmd, EzyArray data)
		{
			throw new NotImplementedException();
		}

		public string getName()
		{
			return this.config.getClientName();
		}

		public EzyClientConfig getConfig()
		{
			return this.config;
		}

		public EzyUser getMe()
		{
			return this.me;
		}

		public EzyZone getZone()
		{
			return this.zone;
		}

		public EzyConnectionStatus getStatus()
		{
			return this.status;
		}

		public void setStatus(EzyConnectionStatus status)
		{
			this.status = status;
		}

		public bool isConnected()
		{
			return this.status == EzyConnectionStatus.CONNECTED;
		}
		
		public EzyConnectionStatus getUdpStatus()
		{
			throw new NotImplementedException();
		}
		
		public void setUdpStatus(EzyConnectionStatus status)
		{
			throw new NotImplementedException();
		}
		
		public bool isUdpConnected()
		{
			throw new NotImplementedException();
		}
		
		public void setSessionId(long sessionId)
		{
			this.sessionId = sessionId;
		}
		
		public void setSessionToken(string token)
		{
			this.sessionToken = token;
		}
		
		public EzyISocketClient getSocket()
		{
			throw new NotImplementedException();
		}
		
		public EzyApp getApp()
		{
			if (zone != null)
			{
				EzyAppManager appManager = zone.getAppManager();
				EzyApp app = appManager.getApp();
				return app;
			}
			return null;
		}
		
		public EzyApp getAppById(int appId)
		{
			if(zone != null) {
				EzyAppManager appManager = zone.getAppManager();
				EzyApp app = appManager.getAppById(appId);
				return app;
			}
			return null;
		}
		
		public EzyPlugin getPlugin()
		{
			if (zone != null)
			{
				EzyPluginManager pluginManager = zone.getPluginManager();
				EzyPlugin plugin = pluginManager.getPlugin();
				return plugin;
			}
			return null;
		}
		
		public EzyPlugin getPluginById(int pluginId)
		{
			if (zone != null)
			{
				EzyPluginManager pluginManager = zone.getPluginManager();
				EzyPlugin plugin = pluginManager.getPluginById(pluginId);
				return plugin;
			}
			return null;
		}
		
		public EzyPingManager getPingManager()
		{
			return this.pingManager;
		}
		
		public EzyPingSchedule getPingSchedule()
		{
			return this.pingSchedule;
		}

		public EzyHandlerManager getHandlerManager()
		{
			return this.handlerManager;
		}

		public EzyStatistics getNetworkStatistics()
		{
			throw new NotImplementedException();
		}

		public EzyTransportType getTransportType()
		{
			return EzyTransportType.WS;
		}

		public void setMe(EzyUser me)
		{
			this.me = me;
		}

		public void setZone(EzyZone zone)
		{
			this.zone = zone;
		}
	}
}
