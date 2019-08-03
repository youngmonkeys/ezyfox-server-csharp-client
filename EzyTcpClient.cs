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

        protected EzyUser me;
        protected EzyZone zone;
        protected readonly String name;
        protected readonly String zoneName;
        protected readonly EzyClientConfig config;
        protected readonly EzyPingManager pingManager;
        protected readonly EzyHandlerManager handlerManager;
        protected readonly IDictionary<int, EzyApp> appsById;

        protected EzyConnectionStatus status;
        protected readonly Object statusLock;
        protected readonly ISet<Object> unloggableCommands;

        protected readonly EzySocketClient socketClient;
        protected readonly EzyPingSchedule pingSchedule;
        protected readonly EzyMainThreadQueue mainThreadQueue;
        protected readonly EzyLogger logger;

		public EzyTcpClient(EzyClientConfig config)
		{
			this.config = config;
            this.name = config.getClientName();
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

        protected void initProperties()
		{
			this.properties.put(typeof(EzySetup), newSetupCommand());
		}

        protected EzyHandlerManager newHandlerManager()
		{
			return new EzySimpleHandlerManager(this, pingSchedule);
		}

        protected ISet<Object> newUnloggableCommands()
		{
			ISet<Object> set = new HashSet<Object>();
			set.Add(EzyCommand.PING);
			set.Add(EzyCommand.PONG);
			return set;
		}

        protected EzySetup newSetupCommand()
		{
			return new EzySimpleSetup(handlerManager);
		}

        protected EzySocketClient newSocketClient()
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

        protected void resetComponents()
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

        public String getName() 
        {
            return name;    
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
