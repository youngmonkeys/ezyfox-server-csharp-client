using System;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.socket;
using com.tvd12.ezyfoxserver.client.request;

namespace com.tvd12.ezyfoxserver.client
{
    public class EzyUTClient : EzyTcpClient
    {
        public EzyUTClient(EzyClientConfig config) : base(config)
        {
        }

        protected override EzyTcpSocketClient newTcpSocketClient()
        {
            return new EzyUTSocketClient();
        }

        public override void udpConnect(int port)
        {
            ((EzyUTSocketClient)socketClient).udpConnect(port);
        }

        public override void udpConnect(String host, int port)
        {
            ((EzyUTSocketClient)socketClient).udpConnect(host, port);
        }

        public override void udpSend(EzyRequest request)
        {
            udpSend(request, false);
        }

        public override void udpSend(EzyRequest request, bool encrypted)
        {
            Object cmd = request.getCommand();
            EzyData data = request.serialize();
            udpSend((EzyCommand)cmd, (EzyArray)data, encrypted);
        }

        public override void udpSend(EzyCommand cmd, EzyArray data) {
            udpSend(cmd, data, false);
        }

        public override void udpSend(EzyCommand cmd, EzyArray data, bool encrypted)
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
                ((EzyUTSocketClient)socketClient).udpSendMessage(array, shouldEncrypted);
                printSentData(cmd, data);
            }
        }

        public override EzyTransportType getTransportType()
        {
            return EzyTransportType.UDP;
        }
    }
}
