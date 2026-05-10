using System;
using UnityEngine;

namespace com.tvd12.ezyfoxserver.client.unity
{
    [Serializable]
    public class EzySocketConfig
    {
        [SerializeField] private string zoneName;

        [SerializeField] private string appName;

        [SerializeField] private string webSocketUrl;

        [SerializeField] private string tcpUrl;

        [SerializeField] private int udpPort;

        [SerializeField] private bool udpUsage;

        [SerializeField] private bool enableSSL;

        [SerializeField] private bool enableReconnect = true;

        [SerializeField] private int maxReconnectCount = 5;

        [SerializeField] private int reconnectPeriod = 3000;

        public EzySocketConfig(Builder builder)
        {
            zoneName = builder.GetZoneName();
            appName = builder.GetAppName();
            webSocketUrl = builder.GetWebSocketUrl();
            tcpUrl = builder.GetTcpUrl();
            udpPort = builder.GetUdpPort();
            udpUsage = builder.IsUdpUsage();
            enableSSL = builder.IsEnableSSL();
            enableReconnect = builder.IsEnableReconnect();
            maxReconnectCount = builder.GetMaxReconnectCount();
            reconnectPeriod = builder.GetReconnectPeriod();
        }

        private EzySocketConfig() {}

        public string ZoneName
        {
            get => zoneName;
            set => zoneName = value;
        }

        public string AppName
        {
            get => appName;
            set => appName = value;
        }

        public string WebSocketUrl
        {
            get => webSocketUrl;
            set => webSocketUrl = value;
        }

        public string TcpUrl
        {
            get => tcpUrl;
            set => tcpUrl = value;
        }

        public int UdpPort
        {
            get => udpPort;
            set => udpPort = value;
        }

        public bool UdpUsage
        {
            get => udpUsage;
            set => udpUsage = value;
        }

        public bool EnableSSL
        {
            get => enableSSL;
            set => enableSSL = value;
        }

        public bool EnableReconnect
        {
            get => enableReconnect;
            set => enableReconnect = value;
        }

        public int MaxReconnectCount
        {
            get => maxReconnectCount;
            set => maxReconnectCount = value;
        }

        public int ReconnectPeriod
        {
            get => reconnectPeriod;
            set => reconnectPeriod = value;
        }

        public static Builder GetBuilder()
        {
            return new Builder();
        }

        public class Builder
        {
            private string zoneName;
            private string appName;
            private string webSocketUrl;
            private string tcpUrl;
            private int udpPort;
            private bool udpUsage;
            private bool enableSSL;
            private bool enableReconnect = true;
            private int maxReconnectCount = 5;
            private int reconnectPeriod = 3000;

            public string GetZoneName()
            {
                return zoneName;
            }
            
            public string GetAppName()
            {
                return appName;
            }

            public string GetWebSocketUrl()
            {
                return webSocketUrl;
            }

            public string GetTcpUrl()
            {
                return tcpUrl;
            }

            public int GetUdpPort()
            {
                return udpPort;
            }

            public bool IsUdpUsage()
            {
                return udpUsage;
            }

            public bool IsEnableSSL()
            {
                return enableSSL;
            }

            public bool IsEnableReconnect()
            {
                return enableReconnect;
            }

            public int GetMaxReconnectCount()
            {
                return maxReconnectCount;
            }

            public int GetReconnectPeriod()
            {
                return reconnectPeriod;
            }

            public Builder ZoneName(string zoneName)
            {
                this.zoneName = zoneName;
                return this;
            }

            public Builder AppName(string appName)
            {
                this.appName = appName;
                return this;
            }

            public Builder WebSocketUrl(string webSocketUrl)
            {
                this.webSocketUrl = webSocketUrl;
                return this;
            }

            public Builder TcpUrl(string tcpUrl)
            {
                this.tcpUrl = tcpUrl;
                return this;
            }

            public Builder UdpPort(int udpPort)
            {
                this.udpPort = udpPort;
                return this;
            }

            public Builder UdpUsage(bool udpUsage)
            {
                this.udpUsage = udpUsage;
                return this;
            }

            public Builder EnableSSL(bool enableSSL)
            {
                this.enableSSL = enableSSL;
                return this;
            }

            public Builder EnableReconnect(bool enableReconnect)
            {
                this.enableReconnect = enableReconnect;
                return this;
            }

            public Builder MaxReconnectCount(int maxReconnectCount)
            {
                this.maxReconnectCount = maxReconnectCount;
                return this;
            }

            public Builder ReconnectPeriod(int reconnectPeriod)
            {
                this.reconnectPeriod = reconnectPeriod;
                return this;
            }

            public EzySocketConfig Build()
            {
                return new EzySocketConfig(this);
            }
        }
    }
}
