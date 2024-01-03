using System;
using UnityEngine;

namespace com.tvd12.ezyfoxserver.client.unity
{
    [Serializable]
    public struct EzySocketConfig
    {
        [SerializeField] private string zoneName;

        [SerializeField] private string appName;

        [SerializeField] private string webSocketUrl;

        [SerializeField] private string tcpUrl;

        [SerializeField] private int udpPort;

        [SerializeField] private bool udpUsage;

        [SerializeField] private bool enableSSL;

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
    }
}
