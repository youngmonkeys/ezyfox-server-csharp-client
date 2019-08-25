using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.setup;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.manager;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.socket;
using static com.tvd12.ezyfoxserver.client.constant.EzyConnectionStatuses;

namespace com.tvd12.ezyfoxserver.client
{
	public class EzyTcpClient :
		EzyEntity,
		EzyClient, EzyMeAware, EzyZoneAware
	{

        protected EzyUser me;
        protected EzyZone zone;
        protected readonly String name;
        protected readonly EzySetup settingUp;
        protected readonly EzyClientConfig config;
        protected readonly EzyPingManager pingManager;
        protected readonly EzyHandlerManager handlerManager;
        protected readonly EzyRequestSerializer requestSerializer;

        protected EzyConnectionStatus status;
        protected readonly ISet<Object> unloggableCommands;

        protected readonly EzySocketClient socketClient;
        protected readonly EzyPingSchedule pingSchedule;
        protected readonly EzyMainThreadQueue mainThreadQueue;
        protected readonly EzyLogger logger;

		public EzyTcpClient(EzyClientConfig config)
		{
			this.config = config;
            this.name = config.getClientName();
			this.status = EzyConnectionStatus.NULL;
			this.pingManager = new EzySimplePingManager();
			this.pingSchedule = new EzyPingSchedule(this);
            this.handlerManager = new EzySimpleHandlerManager(this);
            this.requestSerializer = new EzySimpleRequestSerializer();
            this.settingUp = new EzySimpleSetup(handlerManager);
            this.unloggableCommands = newUnloggableCommands();
			this.socketClient = newSocketClient();
            this.logger = EzyLoggerFactory.getLogger(GetType());
		}

        protected ISet<Object> newUnloggableCommands()
		{
			ISet<Object> set = new HashSet<Object>();
			set.Add(EzyCommand.PING);
			set.Add(EzyCommand.PONG);
			return set;
		}

        protected EzySocketClient newSocketClient()
		{
			EzyTcpSocketClient client = new EzyTcpSocketClient();
            client.setPingSchedule(pingSchedule);
            client.setHandlerManager(handlerManager);
            client.setReconnectConfig(config.getReconnect());
            client.setUnloggableCommands(unloggableCommands);
			return client;
		}

        public EzySetup setup() {
            return settingUp;
        }

		public void connect(String host, int port)
		{
			try
			{
                if (!isClientConnectable(status))
                {
                    logger.warn("client has already connected to: " + host + ":" + port);
                    return;
                }
                preconnect();
				socketClient.connectTo(host, port);
				setStatus(EzyConnectionStatus.CONNECTING);
			}
			catch (Exception e)
			{
                logger.error("connect to server error", e);
			}
		}

		public bool reconnect()
		{
            if (!isClientReconnectable(status))
            {
                String host = socketClient.getHost();
                int port = socketClient.getPort();
                logger.warn("client has already connected to: " + host + ":" + port);
                return false;
            }
            preconnect();
			bool success = socketClient.reconnect();
			if (success)
				setStatus(EzyConnectionStatus.RECONNECTING);
			return success;
		}

        protected void preconnect()
		{
			this.me = null;
			this.zone = null;
		}

		public void disconnect(int reason)
		{
            socketClient.disconnect(reason);
		}

		public void send(EzyRequest request)
		{
            Object cmd = request.getCommand();
            EzyData data = request.serialize();
            send((EzyCommand)cmd, (EzyArray)data);
		}

        public void send(EzyCommand cmd, EzyArray data)
		{
            EzyArray array = requestSerializer.serialize(cmd, data);
            if(socketClient != null) 
            {
                socketClient.sendMessage(array);
                printSentData(cmd, data);
            }
		}

		public void processEvents()
		{
			mainThreadQueue.polls();
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

		public EzyConnectionStatus getStatus()
		{
            return status;
		}

		public void setStatus(EzyConnectionStatus status)
		{
            this.status = status;
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

		public EzyPingManager getPingManager()
		{
			return pingManager;
		}

        public EzyPingSchedule getPingSchedule() 
        {
            return pingSchedule;
        }

		public EzyHandlerManager getHandlerManager()
		{
			return handlerManager;
		}

        private void printSentData(EzyCommand cmd, EzyArray data)
        {
            if (!unloggableCommands.Contains(cmd))
                logger.debug("send command: " + cmd + " and data: " + data);
        }
	}

}
