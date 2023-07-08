using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.statistics;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzyUTSocketClient : EzyTcpSocketClient
    {
        protected readonly EzyUdpSocketClient udpClient;
    
        public EzyUTSocketClient() : base()
        {
            this.udpClient = new EzyUdpSocketClient(codecFactory);
        }

        public override void setNetworkStatistics(EzyStatistics networkStatistics)
        {
            base.setNetworkStatistics(networkStatistics);
            this.udpClient.setNetworkStatistics(networkStatistics);
        }

        public void udpConnect(int port)
        {
            udpConnect(host, port);
        }

        public void udpConnect(String host, int port)
        {
            this.udpClient.setSessionId(sessionId);
            this.udpClient.setSessionToken(sessionToken);
            this.udpClient.setSessionKey(sessionKey);
            this.udpClient.connectTo(host, port);
        }

        public void udpSendMessage(EzyArray message, bool encrypted)
        {
            this.udpClient.sendMessage(message, encrypted);
        }

        public void udpSetStatus(EzySocketStatus status)
        {
            this.udpClient.setStatus(status);
        }

        protected override void popReadMessages()
        {
            base.popReadMessages();
            this.udpClient.popReadMessages(localMessageQueue);
        }

        protected override void clearComponents(int disconnectReason)
        {
            this.udpClient.disconnect(disconnectReason);
        }

    }
}
