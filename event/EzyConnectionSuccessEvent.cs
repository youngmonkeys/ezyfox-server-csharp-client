namespace com.tvd12.ezyfoxserver.client.evt
{
	public class EzyConnectionSuccessEvent : EzyEvent
	{
		public EzyEventType getType()
		{
			return EzyEventType.CONNECTION_SUCCESS;
		}
	}

}
