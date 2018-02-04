namespace com.tvd12.ezyfoxserver.client.evt
{
	public class EzyConnectionSuccessEvent : EzyEvent
	{
		public int getType()
		{
			return EzyEventType.CONNECTION_SUCCESS;
		}
	}

}
