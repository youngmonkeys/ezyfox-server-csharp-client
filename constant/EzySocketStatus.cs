namespace com.tvd12.ezyfoxserver.client.constant
{
    public enum EzySocketStatus
    {
        NOT_CONNECT = 1,
        CONNECTING,
        CONNECTED,
        CONNECT_FAILED,
        DISCONNECTING,
        DISCONNECTED,
        RECONNECTING
    }

    public sealed class EzySocketStatuses
    {

        private EzySocketStatuses()
        {
        }

        public static bool isSocketConnectable(EzySocketStatus status)
        {
            return status == EzySocketStatus.NOT_CONNECT ||
                   status == EzySocketStatus.DISCONNECTED ||
                   status == EzySocketStatus.CONNECT_FAILED;
        }

        public static bool isSocketDisconnectable(EzySocketStatus status)
        {
            return status == EzySocketStatus.CONNECTED || 
                   status == EzySocketStatus.DISCONNECTING;
        }

        public static bool isSocketReconnectable(EzySocketStatus status)
        {
            return status == EzySocketStatus.DISCONNECTED ||
                   status == EzySocketStatus.CONNECT_FAILED;
        }

    }
}