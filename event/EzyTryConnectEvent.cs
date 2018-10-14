namespace com.tvd12.ezyfoxserver.client.evt
{
	public class EzyTryConnectEvent : EzyEvent
	{
		private readonly int count;

		public EzyTryConnectEvent(int count)
		{
			this.count = count;
		}

		public int getCount()
		{
			return count;
		}

		public EzyEventType getType()
		{
			return EzyEventType.TRY_CONNECT;
		}
	}

}
