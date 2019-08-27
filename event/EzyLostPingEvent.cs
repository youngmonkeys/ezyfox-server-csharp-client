namespace com.tvd12.ezyfoxserver.client.evt
{
	public class EzyLostPingEvent : EzyEvent
	{
		private readonly int count;

		public EzyLostPingEvent(int count)
		{
			this.count = count;
		}

		public int getCount()
		{
			return count;
		}

		public EzyEventType getType()
		{
			return EzyEventType.LOST_PING;
		}
	}
}
