using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;
using com.tvd12.ezyfoxserver.client.binding;
using com.tvd12.ezyfoxserver.client.concurrent;
using com.tvd12.ezyfoxserver.client.evt;

namespace com.tvd12.ezyfoxserver.client.support
{
    public class EzySocketProxy
    {
        private String host = "127.0.0.1";
        private int tcpPort = 3005;
        private int udpPort = 2611;
        private EzyTransportType transportType;
        private String loginUsername;
        private String loginPassword;
        private String defaultAppName;
        private IDictionary<String, Object> loginData;
        private Type loginResponseDataType;
        private Type loginErrorDataType;
        private Type appAccessedDataType;
        private EzyClient client;
        private readonly String zoneName;
        private readonly EzyBinding binding;
        private readonly AtomicBoolean inited;
        private readonly IDictionary<String, EzyAppProxy> appProxyByName;
        private readonly IDictionary<Object, DataHandler> loginSuccessHandlers;
        private readonly IDictionary<Object, DataHandler> loginErrorHandlers;
        private readonly IDictionary<Object, AppProxyDataHandler> appAccessedHandlers;
        private readonly IDictionary<Object, EventHandler> disconnectedHandlers;
        private readonly IDictionary<Object, EventHandler> reconnectingHandlers;
        private readonly IDictionary<Object, EventHandler> pingLostHandlers;

        public EzySocketProxy(String zoneName, EzyBinding binding)
        {
            this.zoneName = zoneName;
            this.binding = binding;
            this.inited = new AtomicBoolean();
            this.transportType = EzyTransportType.TCP;
            this.appProxyByName = new Dictionary<String, EzyAppProxy>();
            this.loginSuccessHandlers = new Dictionary<Object, DataHandler>();
            this.loginErrorHandlers = new Dictionary<Object, DataHandler>();
            this.appAccessedHandlers = new Dictionary<Object, AppProxyDataHandler>();
            this.disconnectedHandlers = new Dictionary<Object, EventHandler>();
            this.reconnectingHandlers = new Dictionary<Object, EventHandler>();
            this.pingLostHandlers = new Dictionary<Object, EventHandler>();
        }

        public EzySocketProxy setHost(String host)
        {
            this.host = host;
            return this;
        }

        public EzySocketProxy setTcpPort(int tcpPort)
        {
            this.tcpPort = tcpPort;
            return this;
        }

        public EzySocketProxy setUdpPort(int udpPort)
        {
            this.udpPort = udpPort;
            return this;
        }

        public EzySocketProxy setTransportType(EzyTransportType transportType)
        {
            this.transportType = transportType;
            return this;
        }

        public EzySocketProxy setLoginUsername(String loginUsername)
        {
            this.loginUsername = loginUsername;
            return this;
        }

        public EzySocketProxy setLoginPassword(String loginPassword)
        {
            this.loginPassword = loginPassword;
            return this;
        }

        public EzySocketProxy setLoginData(IDictionary<String, Object> loginData)
        {
            this.loginData = loginData;
            return this;
        }

        public EzySocketProxy setDefaultAppName(String defaultAppName)
        {
            this.defaultAppName = defaultAppName;
            return this;
        }

        public EzyClient getClient()
        {
            return client;
        }

        public EzyBinding getBinding()
        {
            return binding;
        }

        public EzyAppProxy getDefaultAppProxy()
        {
            if (defaultAppName == null)
            {
                throw new Exception("Must set defaultAppName ahead");
            }
            return getAppProxy(defaultAppName, true);
        }

        public EzyAppProxy getAppProxy(String appName)
        {
            return getAppProxy(appName, false);
        }

        public EzyAppProxy getAppProxy(
            String appName,
            bool createIfNotExists
        )
        {
            EzyAppProxy appProxy = appProxyByName.ContainsKey(appName)
                ? appProxyByName[appName]
                : null;
            if (appProxy == null && createIfNotExists)
            {
                init();
                appProxy = new EzySimpleAppProxy(this, appName);
                appProxyByName[appName] = appProxy;
            }
            return appProxy;
        }

        public void connect()
        {
            this.init();
            this.client.connect(host, tcpPort);
        }

        private void init()
        {
            if (inited.compareAndSet(false, true))
            {
                EzyClientConfig clientConfig = EzyClientConfig
                    .builder()
                    .clientName(zoneName)
                    .zoneName(zoneName)
                    .build();
                EzyClients clients = EzyClients.getInstance();
                this.client = clients.getClient(zoneName);
                if (client == null)
                {
                    this.client = transportType == EzyTransportType.UDP
                        ? new EzyUTClient(clientConfig)
                        : new EzyTcpClient(clientConfig);
                    clients.addClient(client);
                }
                client.setup()
                    .addEventHandler(EzyEventType.DISCONNECTION, new DisconnectionHandler(this))
                    .addEventHandler(EzyEventType.LOST_PING, new PingLostHandler(this))
                    .addDataHandler(EzyCommand.HANDSHAKE, new HandshakeEventHandler(this))
                    .addDataHandler(EzyCommand.LOGIN, new LoginSuccessHandler(this))
                    .addDataHandler(EzyCommand.LOGIN_ERROR, new LoginErrorHandler(this))
                    .addDataHandler(EzyCommand.APP_ACCESS, new AppAccessHandler(this))
                    .addDataHandler(EzyCommand.UDP_HANDSHAKE, new UdpHandshakeHandler(this));
            }
        }

        public void send(EzyRequest request)
        {
            client.send(request);
        }

        public void send(EzyCommand cmd, Object data)
        {
            client.send(cmd, binding.marshall<EzyArray>(data));
        }

        public void udpSend(EzyRequest request)
        {
            client.udpSend(request);
        }

        public void udpSend(EzyCommand cmd, EzyArray data)
        {
            client.udpSend(cmd, binding.marshall<EzyArray>(data));
        }

        public Object onLoginSuccess<T>(EzySocketProxyDataHandler<T> handler)
        {
            DataHandler dataHandler = data =>
            {
                handler.Invoke(this, (T)data);
            };
            loginResponseDataType = typeof(T);
            loginSuccessHandlers[handler] = dataHandler;
            return handler;
        }

        public Object onLoginError<T>(EzySocketProxyDataHandler<T> handler)
        {
            DataHandler dataHandler = data =>
            {
                handler.Invoke(this, (T)data);
            };
            loginErrorDataType = typeof(T);
            loginErrorHandlers[handler] = dataHandler;
            return handler;
        }

        public Object onAppAccessed<T>(EzyAppProxyDataHandler<T> handler)
        {
            AppProxyDataHandler dataHandler = (appProxy, data) =>
            {
                handler.Invoke(appProxy, (T)data);
            };
            appAccessedDataType = typeof(T);
            appAccessedHandlers[handler] = dataHandler;
            return handler;
        }

        public Object onDisconnected(
            EzySocketProxyEventHandler<EzyDisconnectionEvent> handler
        )
        {
            EventHandler dataHandler = evt =>
            {
                handler.Invoke(this, (EzyDisconnectionEvent)evt);
            };
            disconnectedHandlers[handler] = dataHandler;
            return handler;
        }

        public Object onReconnecting(
            EzySocketProxyEventHandler<EzyDisconnectionEvent> handler
        )
        {
            EventHandler dataHandler = evt =>
            {
                handler.Invoke(this, (EzyDisconnectionEvent)evt);
            };
            reconnectingHandlers[handler] = dataHandler;
            return handler;
        }

        public Object onPingLost(
            EzySocketProxyEventHandler<EzyLostPingEvent> handler
        )
        {
            EventHandler dataHandler = evt =>
            {
                handler.Invoke(this, (EzyLostPingEvent)evt);
            };
            pingLostHandlers[handler] = dataHandler;
            return handler;
        }

        public void unbind(Object handler)
        {
            loginSuccessHandlers.Remove(handler);
            loginErrorHandlers.Remove(handler);
            appAccessedHandlers.Remove(handler);
            disconnectedHandlers.Remove(handler);
            reconnectingHandlers.Remove(handler);
            pingLostHandlers.Remove(handler);
        }

        public class HandshakeEventHandler : EzyHandshakeHandler
        {

            private readonly EzySocketProxy parent;

            public HandshakeEventHandler(EzySocketProxy parent)
            {
                this.parent = parent;
            }

            protected override EzyRequest getLoginRequest()
            {
                EzyObject data = EzyEntityFactory.newObject();
                if (parent.loginData != null)
                {
                    data.putAll(parent.loginData);
                }
                return new EzyLoginRequest(
                    parent.zoneName,
                    parent.loginUsername,
                    parent.loginPassword,
                    data
                );
            }
        }

        public class LoginSuccessHandler : EzyLoginSuccessHandler
        {
            private readonly EzySocketProxy parent;

            public LoginSuccessHandler(EzySocketProxy parent)
            {
                this.parent = parent;
            }

            protected override void handleLoginSuccess(EzyData responseData)
            {
                if (parent.transportType == EzyTransportType.UDP)
                {
                    this.client.udpConnect(parent.udpPort);
                }
                else if (parent.defaultAppName != null)
                {
                    client.send(new EzyAppAccessRequest(parent.defaultAppName));
                }
                else if (parent.loginSuccessHandlers.Count > 0)
                {
                    Object data = parent.binding.unmarshall(
                        responseData,
                        parent.loginResponseDataType
                    );
                    foreach (DataHandler handler in parent.loginSuccessHandlers.Values)
                    {
                        handler.Invoke(data);
                    }
                }
            }
        }

        public class LoginErrorHandler : EzyLoginErrorHandler
        {
            private readonly EzySocketProxy parent;

            public LoginErrorHandler(EzySocketProxy parent)
            {
                this.parent = parent;
            }

            protected override void handleLoginError(EzyArray errorData)
            {
                if (parent.loginErrorHandlers.Count > 0)
                {
                    Object data = parent.binding.unmarshall(
                        errorData,
                        parent.loginErrorDataType
                    );
                    foreach (DataHandler handler in parent.loginErrorHandlers.Values)
                    {
                        handler.Invoke(data);
                    }
                }
            }
        }

        public class UdpHandshakeHandler : EzyUdpHandshakeHandler
        {

            private readonly EzySocketProxy parent;

            public UdpHandshakeHandler(EzySocketProxy parent)
            {
                this.parent = parent;
            }

            protected override void onAuthenticated(EzyArray data)
            {
                if (parent.defaultAppName != null)
                {
                    client.send(new EzyAppAccessRequest(parent.defaultAppName));
                }
            }
        }

        public class AppAccessHandler : EzyAppAccessHandler
        {
            private readonly EzySocketProxy parent;

            public AppAccessHandler(EzySocketProxy parent)
            {
                this.parent = parent;
            }

            protected override void postHandle(EzyApp app, EzyArray appData)
            {
                EzySimpleAppProxy appProxy =
                    (EzySimpleAppProxy)parent.getAppProxy(app.getName(), true);
                appProxy.setApp(app);
                parent.appProxyByName[app.getName()] = appProxy;
                if (parent.appAccessedHandlers.Count > 0)
                {
                    Object data = parent.binding.unmarshall(
                        appData,
                        parent.appAccessedDataType
                    );
                    foreach (AppProxyDataHandler handler in parent.appAccessedHandlers.Values)
                    {
                        handler.Invoke(appProxy, data);
                    }
                }
            }
        }

        public class DisconnectionHandler : EzyDisconnectionHandler
        {
            private readonly EzySocketProxy parent;

            public DisconnectionHandler(EzySocketProxy parent)
            {
                this.parent = parent;
            }

            protected override void onDisconnected(EzyDisconnectionEvent evt)
            {
                foreach (EventHandler handler in parent.disconnectedHandlers.Values)
                {
                    handler.Invoke(evt);
                }
            }

            protected override void onReconnecting(EzyDisconnectionEvent evt)
            {
                foreach (EventHandler handler in parent.reconnectingHandlers.Values)
                {
                    handler.Invoke(evt);
                }
            }
        }

        public class PingLostHandler : EzyAbstractEventHandler<EzyLostPingEvent>
        {
            private readonly EzySocketProxy parent;

            public PingLostHandler(EzySocketProxy parent)
            {
                this.parent = parent;
            }

            protected override void process(EzyLostPingEvent evt)
            {
                foreach (EventHandler handler in parent.pingLostHandlers.Values)
                {
                    handler.Invoke(evt);
                }
            }
        }
    }
}

// EzySupportFunctions
namespace com.tvd12.ezyfoxserver.client.support
{
    public delegate void DataHandler(object data);

    public delegate void EventHandler(object evt);

    public delegate void AppProxyDataHandler(
        EzyAppProxy appProxy,
        object data
    );

    public delegate void EzySocketProxyDataHandler<T>(
        EzySocketProxy socketProxy,
        T data
    );

    public delegate void EzyAppProxyDataHandler<T>(
        EzyAppProxy appProxy,
        T data
    );

    public delegate void EzySocketProxyEventHandler<E>(
        EzySocketProxy socketProxy,
        E evt
    );
}
