using com.tvd12.ezyfoxserver.client.socket;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.handler
{
    public class EzyUdpHandshakeHandler : EzyAbstractDataHandler
    {

        public override void handle(EzyArray data)
        {
            int responseCode = data.get<int>(0);
            EzyUTSocketClient socket = (EzyUTSocketClient)client.getSocket();
            if (responseCode == EzyStatusCodes.OK)
            {
                socket.udpSetStatus(EzySocketStatus.CONNECTED);
                onAuthenticated(data);
            }
            else
            {
                socket.udpSetStatus(EzySocketStatus.CONNECT_FAILED);
                onAuthenticationError(data);
            }
        }

        protected virtual void onAuthenticated(EzyArray data)
        {
            logger.info("udp authenticated");
        }

        protected virtual void onAuthenticationError(EzyArray data)
        {
            int responseCode = data.get<int>(0);
            logger.info("udp authentication error: " + responseCode);
        }
    }
}
