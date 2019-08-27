namespace com.tvd12.ezyfoxserver.client.evt
{
	public class EzyDisconnectionEvent : EzyEvent
	{
		private readonly int reason;

		public EzyDisconnectionEvent(int reason)
		{
			this.reason = reason;
		}

		public int getReason()
		{
			return reason;
		}

		public EzyEventType getType()
		{
			return EzyEventType.DISCONNECTION;
		}
	}
}
