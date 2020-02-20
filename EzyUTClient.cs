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
            Object cmd = request.getCommand();
            EzyData data = request.serialize();
            send((EzyCommand)cmd, (EzyArray)data);
        }

        public override void udpSend(EzyCommand cmd, EzyArray data)
        {
            EzyArray array = requestSerializer.serialize(cmd, data);
            if (socketClient != null)
            {
                ((EzyUTSocketClient)socketClient).udpSendMessage(array);
                printSentData(cmd, data);
            }
        }
    }
}
