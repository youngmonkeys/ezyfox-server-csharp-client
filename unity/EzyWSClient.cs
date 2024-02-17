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
using Newtonsoft.Json;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public class EzyWSClient : EzyClient, EzyMeAware, EzyZoneAware
	{
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

		private static bool jsDebug = false;
		private static readonly EzyLogger LOGGER =
			EzyUnityLoggerFactory.getLogger(typeof(EzyWSClient));

		public EzyWSClient(EzyClientConfig config)
		{
			this.config = config;
			this.status = EzyConnectionStatus.NULL;
			this.pingManager = new EzySimplePingManager(config.getPing());
			this.pingSchedule = new EzyWSPingSchedule(this);
			this.handlerManager = new EzySimpleHandlerManager(this);
			this.settingUp = new EzySimpleSetup(handlerManager);
		}

		public static void setJsDebug(bool value)
		{
			jsDebug = value;
		}

		public static bool isMobile()
		{
#if UNITY_WEBGL && !UNITY_EDITOR
			return EzyWSProxy.isMobile();
#else
			return false;
#endif
		}

		public void init()
		{
			String configJson = JsonConvert.SerializeObject(
				config,
				new EzyClientConfigJsonConverter()
			);
			EzyWSProxy.setEventHandlerCallback(eventHandlerCallback);
			EzyWSProxy.setDataHandlerCallback(dataHandlerCallback);
			EzyWSProxy.setDebug(jsDebug);
			EzyWSProxy.run4(
				config.getClientName(),
				"init",
				configJson,
				initCallback
			);
		}

		[MonoPInvokeCallback(typeof(EzyDelegates.Delegate2))]
		public static void initCallback(String clientName, String jsonData)
		{
			LOGGER.debug(
				"initCallback: clientName = " +
				clientName + ", jsonData = " + jsonData
			);
		}

		[MonoPInvokeCallback(typeof(EzyDelegates.EventHandlerDelegate))]
		public static void eventHandlerCallback(
			String clientName,
			String eventType,
			String jsonData
		)
		{
			LOGGER.debug(
				"eventHandlerCallback: clientName = " +
				clientName + ", eventType = " + eventType +
				", jsonData = " + jsonData
			);
			var eventJsDataDeserializer = EzyEventWSDataDeserializer
				.getInstance();
			var ezyEvent = eventJsDataDeserializer.deserializeEvent(
				eventType,
				jsonData
			);
			EzyClients.getInstance()
				.getClient(clientName)
				.getHandlerManager()
				.getEventHandler(ezyEvent.getType())
				.handle(ezyEvent);
		}

		[MonoPInvokeCallback(typeof(EzyDelegates.EventHandlerDelegate))]
		public static void dataHandlerCallback(
			String clientName,
			int commandId,
			String jsonData
		)
		{
			LOGGER.debug(
				"dataHandlerCallback: clientName = " +
				clientName + ", commandId = " +
				commandId + ", jsonData = " + jsonData
			);
			var ezyData = EzyJsons.deserialize(jsonData);
			var command = (EzyCommand)commandId;
			var dataHandler = EzyClients.getInstance()
				.getClient(clientName)
				.getHandlerManager()
				.getDataHandler(command);
			if (dataHandler == null)
			{
				LOGGER.warn("has no handler for command: " + command);
			}
			else
			{
				dataHandler.handle((EzyArray)ezyData);
			}
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
			throw new InvalidOperationException("not supported");
		}

		public bool reconnect()
		{
			EzyWSProxy.run3(
				config.getClientName(),
				"reconnect",
				reconnectCallback
			);
			return true;
		}

		public void send(EzyRequest request, bool encrypted = false)
		{
			send(request);
		}

		public void send(EzyCommand cmd, EzyArray data, bool encrypted = false)
		{
			send(cmd, data);
		}

		[MonoPInvokeCallback(typeof(EzyDelegates.Delegate2))]
		public static void reconnectCallback(
			String clientName,
			String jsonData
		)
		{
			LOGGER.debug(
				"reconnectCallback: clientName = " +
				clientName + ", jsonData = " + jsonData
			);
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
			EzyWSProxy.run4(
				config.getClientName(),
				"send",
				jsonData,
				sendCallback
			);
		}

		[MonoPInvokeCallback(typeof(EzyDelegates.Delegate2))]
		public static void sendCallback(String clientName, String jsonData)
		{
			LOGGER.debug(
				"sendCallback: clientName = " +
				clientName + ", jsonData = " + jsonData
			);
		}

		public void disconnect(int reason)
		{
			EzyObject obj = EzyEntityFactory.newObjectBuilder()
				.append("reason", reason)
				.build();
			String jsonData = EzyJsons.serialize(obj);
			EzyWSProxy.run4(
				config.getClientName(),
				"disconnect",
				jsonData,
				null
			);
		}

		public void close()
		{
			disconnect((int) EzyDisconnectReason.CLOSE);
		}
		
		public void processEvents()
		{
			throw new InvalidOperationException("not supported");
		}

		public void udpConnect(int port)
		{
			throw new InvalidOperationException("not supported");
		}

		public void udpConnect(string host, int port)
		{
			throw new InvalidOperationException("not supported");
		}

		public void udpSend(EzyRequest request, bool encrypted = false)
		{
			throw new InvalidOperationException("not supported");
		}

		public void udpSend(EzyCommand cmd, EzyArray data, bool encrypted = false)
		{
			throw new InvalidOperationException("not supported");
		}

		public void udpSend(EzyRequest request)
		{
			throw new InvalidOperationException("not supported");
		}

		public void udpSend(EzyCommand cmd, EzyArray data)
		{
			throw new InvalidOperationException("not supported");
		}

		public string getName()
		{
			return this.config.getClientName();
		}

		public EzyClientConfig getConfig()
		{
			return this.config;
		}

		/**
		 * This method specifies whether SSL is enabled for TCP/UDP. Therefore, it returns false
		 * by default for WebSocket.
		 */
		public bool isEnableSSL()
		{
			return false;
		}

		public bool isEnableDebug()
		{
			return this.config.isEnableDebug();
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
			throw new InvalidOperationException("not supported");
		}
		
		/**
		 * This method sets UDP status, so it should do nothing for WebSocket
		 */
		public void setUdpStatus(EzyConnectionStatus status)
		{
		}
		
		public bool isUdpConnected()
		{
			throw new InvalidOperationException("not supported");
		}
		
		public void setSessionId(long sessionId)
		{
			this.sessionId = sessionId;
		}
		
		public void setSessionToken(string token)
		{
			this.sessionToken = token;
		}

		public void setSessionKey(byte[] sessionKey)
		{
			throw new InvalidOperationException("not supported");
		}

		public byte[] getSessionKey()
		{
			throw new InvalidOperationException("not supported");
		}

		public void setPrivateKey(byte[] privateKey)
		{
			throw new InvalidOperationException("not supported");
		}

		public byte[] getPrivateKey()
		{
			throw new InvalidOperationException("not supported");
		}

		public void setPublicKey(byte[] publicKey)
		{
			throw new InvalidOperationException("not supported");
		}

		public byte[] getPublicKey()
		{
			throw new InvalidOperationException("not supported");
		}
		
		public EzyISocketClient getSocket()
		{
			throw new InvalidOperationException("not supported");
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
			throw new InvalidOperationException("not supported");
		}

		public EzyTransportType getTransportType()
		{
			return EzyTransportType.TCP;
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
