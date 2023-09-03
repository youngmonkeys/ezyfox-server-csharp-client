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
using com.tvd12.ezyfoxserver.client.statistics;
using com.tvd12.ezyfoxserver.client.concurrent;

namespace com.tvd12.ezyfoxserver.client
{
	public class EzyTcpClient :
		EzyEntity,
		EzyClient, EzyMeAware, EzyZoneAware
	{

        protected EzyUser me;
        protected EzyZone zone;
        protected long sessionId;
        protected byte[] publicKey;
        protected byte[] privateKey;
        protected byte[] sessionKey;
        protected String sessionToken;
        protected readonly String name;
        protected readonly EzySetup settingUp;
        protected readonly EzyClientConfig config;
        protected readonly EzyPingManager pingManager;
        protected readonly EzyHandlerManager handlerManager;
        protected readonly EzyStatistics networkStatistics;
        protected readonly EzyRequestSerializer requestSerializer;

        protected EzyConnectionStatus status;
        protected EzyConnectionStatus udpStatus;
        protected readonly ISet<Object> unloggableCommands;

        protected readonly EzySocketClient socketClient;
        protected readonly EzyPingSchedule pingSchedule;
        protected readonly EzyLogger logger;
        protected readonly EzyEventLoopGroup eventLoopGroup;

        public EzyTcpClient(EzyClientConfig config) : this(config, null)
        {
        }

		public EzyTcpClient(
            EzyClientConfig config,
            EzyEventLoopGroup eventLoopGroup
        )
		{
			this.config = config;
            this.name = config.getClientName();
			this.status = EzyConnectionStatus.NULL;
            this.status = EzyConnectionStatus.NULL;
            this.eventLoopGroup = eventLoopGroup;
            this.pingManager = new EzySimplePingManager(config.getPing());
			this.pingSchedule = new EzyPingSchedule(this, eventLoopGroup);
            this.handlerManager = new EzySimpleHandlerManager(this);
            this.networkStatistics = new EzySimpleStatistics();
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
            EzyTcpSocketClient client = newTcpSocketClient();
            client.setPingSchedule(pingSchedule);
            client.setPingManager(pingManager);
            client.setNetworkStatistics(networkStatistics);
            client.setHandlerManager(handlerManager);
            client.setReconnectConfig(config.getReconnect());
            client.setUnloggableCommands(unloggableCommands);
			return client;
		}

        protected virtual EzyTcpSocketClient newTcpSocketClient()
        {
            return new EzyTcpSocketClient();
        }

        public EzySetup setup() {
            return settingUp;
        }

        public void connect(String url)
        {
            String host = url;
            int port = 3005;
            int lastIndex = url.IndexOf(":");
            if (lastIndex > 0)
            {
                host = url.Substring(0, lastIndex);
                port = Int32.Parse(url.Substring(lastIndex + 1));
            }
            connect(host, port);
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
            {
                setStatus(EzyConnectionStatus.RECONNECTING);
            }
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

        public void close()
        {
            disconnect((int) EzyDisconnectReason.CLOSE);
        }

        public void send(EzyRequest request, bool encrypted)
		{
            Object cmd = request.getCommand();
            EzyData data = request.serialize();
            send((EzyCommand)cmd, (EzyArray)data, encrypted);
		}

        public void send(EzyCommand cmd, EzyArray data, bool encrypted)
        {
            bool shouldEncrypted = encrypted;
            if (encrypted && sessionKey == null)
            {
                if (config.isEnableDebug())
                {
                    shouldEncrypted = false;
                }
                else
                {
                    throw new ArgumentException(
                        "can not send command: " + cmd + " " +
                            "you must enable SSL or enable debug mode by configuration " +
                            "when you create the client"
                    );
                }

            }
            EzyArray array = requestSerializer.serialize(cmd, data);
            if (socketClient != null)
            {
                socketClient.sendMessage(array, shouldEncrypted);
                printSentData(cmd, data);
            }
        }

        public void processEvents()
		{
            socketClient.processEventMessages();
		}

        public String getName() 
        {
            return name;    
        }

		public EzyClientConfig getConfig()
		{
			return config;
		}

        public bool isEnableSSL()
        {
            return config.isEnableSSL();
        }

        public bool isEnableDebug()
        {
            return config.isEnableDebug();
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

        public bool isConnected()
        {
            return this.status == EzyConnectionStatus.CONNECTED;
        }

        public void setUdpStatus(EzyConnectionStatus status)
        {
            this.udpStatus = status;
        }

        public EzyConnectionStatus getUdpStatus()
        {
            return this.udpStatus;
        }

        public virtual bool isUdpConnected()
        {
            return udpStatus == EzyConnectionStatus.CONNECTED;
        }

        public void setSessionId(long sessionId)
        {
            this.sessionId = sessionId;
            this.socketClient.setSessionId(sessionId);
        }

        public void setSessionToken(String token)
        {
            this.sessionToken = token;
            this.socketClient.setSessionToken(sessionToken);
        }

        public void setSessionKey(byte[] sessionKey)
        {
            this.sessionKey = sessionKey;
            this.socketClient.setSessionKey(sessionKey);
        }

        public byte[] getSessionKey()
        {
            return sessionKey;
        }

        public void setPublicKey(byte[] publicKey)
        {
            this.publicKey = publicKey;
        }

        public byte[] getPublicKey()
        {
            return publicKey;
        }

        public void setPrivateKey(byte[] privateKey)
        {
            this.privateKey = privateKey;
        }

        public byte[] getPrivateKey()
        {
            return privateKey;
        }


        public EzyISocketClient getSocket() 
        {
            return socketClient;    
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

        protected void printSentData(EzyCommand cmd, EzyArray data)
        {
            if (!unloggableCommands.Contains(cmd))
            {
                logger.debug("send command: " + cmd + " and data: " + data);
            }
        }

        public virtual void udpConnect(int port)
        {
            throw new InvalidOperationException("only support TCP, use EzyUTClient instead");
        }

        public virtual void udpConnect(String host, int port)
        {
            throw new InvalidOperationException("only support TCP, use EzyUTClient instead");
        }
            
        public virtual void udpSend(EzyRequest request, bool encrypted)
        {
            throw new InvalidOperationException("only support TCP, use EzyUTClient instead");
        }

        public virtual void udpSend(EzyCommand cmd, EzyArray data, bool encrypted)
        {
            throw new InvalidOperationException("only support TCP, use EzyUTClient instead");
        }

        public EzyStatistics getNetworkStatistics()
        {
            return networkStatistics;
        }

        public virtual EzyTransportType getTransportType()
        {
            return EzyTransportType.TCP;
        }
    }

}
