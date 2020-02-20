using System;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public class EzyUTSocketClient : EzyTcpSocketClient
    {
        protected readonly EzyUdpSocketClient udpClient;
    
        public EzyUTSocketClient() : base()
        {
            this.udpClient = new EzyUdpSocketClient(codecFactory);
        }

        public void udpConnect(int port)
        {
            udpConnect(host, port);
        }

        public void udpConnect(String host, int port)
        {
            this.udpClient.setSessionId(sessionId);
            this.udpClient.setSessionToken(sessionToken);
            this.udpClient.connectTo(host, port);
        }

        public void udpSendMessage(EzyArray message)
        {
            this.udpClient.sendMessage(message);
        }

        protected override void popReadMessages()
        {
            base.popReadMessages();
            this.udpClient.popReadMessages(localMessageQueue);
        }

        public override void onDisconnected(int reason)
        {
            base.onDisconnected(reason);
            this.udpClient.disconnect(reason);
        }
    }
}
