namespace com.tvd12.ezyfoxserver.client.constant
{
	public enum EzyConnectionStatus
	{
		NULL = 0,
		CONNECTING = 1,
		CONNECTED = 2,
		DISCONNECTED = 3,
		FAILURE = 4,
		RECONNECTING = 5
	}

    public sealed class EzyConnectionStatuses {
        
        private EzyConnectionStatuses() { 
        }            

        public static bool isClientConnectable(EzyConnectionStatus status) {
            return status == EzyConnectionStatus.NULL ||
                   status == EzyConnectionStatus.DISCONNECTED ||
                   status == EzyConnectionStatus.FAILURE;
        }

        public static bool isClientReconnectable(EzyConnectionStatus status)
        {
            return status == EzyConnectionStatus.DISCONNECTED ||
                   status == EzyConnectionStatus.FAILURE;
        }

    }
}
