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

        public EzySocketConfig(Builder builder)
        {
            zoneName = builder.GetZoneName();
            appName = builder.GetAppName();
            webSocketUrl = builder.GetWebSocketUrl();
            tcpUrl = builder.GetTcpUrl();
            udpPort = builder.GetUdpPort();
            udpUsage = builder.IsUdpUsage();
            enableSSL = builder.IsEnableSSL();
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

            public EzySocketConfig Build()
            {
                return new EzySocketConfig(this);
            }
        }
    }
}
