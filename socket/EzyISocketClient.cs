using System;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.socket
{
    public interface EzyISocketClient
    {
        void connectTo(String host, int port);

        bool reconnect();

        void disconnect(int reason);

        void sendMessage(EzyArray message, bool encrypted);
    }
}
