using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.command;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.manager;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.socket;

namespace com.tvd12.ezyfoxserver.client
{
	public class EzyTcpClient :
		EzyEntity,
		EzyClient, EzyMeAware, EzyZoneAware
	{

		private EzyUser me;
		private EzyZone zone;
		private readonly String zoneName;
		private readonly EzyClientConfig config;
		private readonly EzyPingManager pingManager;
		private readonly EzyHandlerManager handlerManager;
		private readonly IDictionary<int, EzyApp> appsById;

		private EzyConnectionStatus status;
		private readonly Object statusLock;
		private readonly ISet<Object> unloggableCommands;

		private readonly EzySocketClient socketClient;
		private readonly EzyPingSchedule pingSchedule;
		private readonly EzyMainThreadQueue mainThreadQueue;
        private readonly EzyLogger logger;

		public EzyTcpClient(EzyClientConfig config)
		{
			this.config = config;
			this.zoneName = config.getZoneName();
			this.status = EzyConnectionStatus.NULL;
			this.statusLock = new Object();
			this.unloggableCommands = newUnloggableCommands();
			this.pingManager = new EzySimplePingManager();
			this.appsById = new Dictionary<int, EzyApp>();
			this.pingSchedule = new EzyPingSchedule(this);
			this.mainThreadQueue = new EzyMainThreadQueue();
			this.handlerManager = newHandlerManager();
			this.socketClient = newSocketClient();
            this.logger = EzyLoggerFactory.getLogger(GetType());
			this.initProperties();
		}

		private void initProperties()
		{
			this.properties.put(typeof(EzySetup), newSetupCommand());
		}

		private EzyHandlerManager newHandlerManager()
		{
			return new EzySimpleHandlerManager(this, pingSchedule);
		}

		private ISet<Object> newUnloggableCommands()
		{
			ISet<Object> set = new HashSet<Object>();
			//set.Add(EzyCommand.PING);
			//set.Add(EzyCommand.PONG);
			return set;
		}

		private EzySetup newSetupCommand()
		{
			return new EzySimpleSetup(handlerManager);
		}

		private EzySocketClient newSocketClient()
		{
			EzyTcpSocketClient client = new EzyTcpSocketClient(
					config,
					mainThreadQueue,
					handlerManager,
					pingManager,
					pingSchedule, unloggableCommands);
			return client;
		}

		public void connect(String host, int port)
		{
			try
			{
				resetComponents();
				socketClient.connect(host, port);
				setStatus(EzyConnectionStatus.CONNECTING);
			}
			catch (Exception e)
			{
                logger.error("connect to server error", e);
			}
		}

		public void connect()
		{
			resetComponents();
			socketClient.connect();
			setStatus(EzyConnectionStatus.CONNECTING);
		}

		public bool reconnect()
		{
			resetComponents();
			bool success = socketClient.reconnect();
			if (success)
				setStatus(EzyConnectionStatus.RECONNECTING);
			return success;
		}

		private void resetComponents()
		{
			this.me = null;
			this.zone = null;
		}

		public void disconnect()
		{
			socketClient.disconnect();
			setStatus(EzyConnectionStatus.DISCONNECTED);
		}

		public void send(EzyRequest request)
		{
			socketClient.send(request);
		}

		public void send(Object cmd, EzyData data)
		{
			socketClient.send(cmd, data);
		}

		public void processEvents()
		{
			mainThreadQueue.polls();
		}

		public T get<T>()
		{
			T instance = getProperty<T>();
			return instance;
		}

		public EzyClientConfig getConfig()
		{
			return config;
		}

		public EzyZone getZone()
		{
			return zone;
		}

		public void setZone(EzyZone zone)
		{
			this.zone = zone;
		}

		public EzyUser getMe()
		{
			return me;
		}

		public void setMe(EzyUser me)
		{
			this.me = me;
		}

		public String getZoneName()
		{
			return zoneName;
		}

		public EzyConnectionStatus getStatus()
		{
			lock (statusLock)
			{
				return status;
			}
		}

		public void setStatus(EzyConnectionStatus status)
		{
			lock (statusLock)
			{
				this.status = status;
			}
		}

		public void addApp(EzyApp app)
		{
			appsById[app.getId()] = app;
		}

		public EzyApp getAppById(int appId)
		{
			if (appsById.ContainsKey(appId))
				return appsById[appId];
			throw new ArgumentException("has no app with id = " + appId);
		}

		public EzyPingManager getPingManager()
		{
			return pingManager;
		}

		public EzyHandlerManager getHandlerManager()
		{
			return handlerManager;
		}
	}

}
