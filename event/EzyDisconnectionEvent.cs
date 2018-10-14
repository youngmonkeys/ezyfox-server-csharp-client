using com.tvd12.ezyfoxserver.client.constant;

namespace com.tvd12.ezyfoxserver.client.evt
{
	public class EzyDisconnectionEvent : EzyEvent
	{
		private readonly EzyDisconnectReason reason;

		public EzyDisconnectionEvent(EzyDisconnectReason reason)
		{
			this.reason = reason;
		}

		public EzyDisconnectReason getReason()
		{
			return reason;
		}

		public EzyEventType getType()
		{
			return EzyEventType.DISCONNECTION;
		}
	}
}
