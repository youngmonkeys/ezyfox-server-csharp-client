using System;
namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySimpleSocketEvent : EzySocketEvent
	{
		private readonly Object data;
		private readonly EzySocketEventType type;

		public EzySimpleSocketEvent(EzySocketEventType type, 
		                            Object data)
		{
			this.type = type;
			this.data = data;
		}

		public EzySocketEventType getType()
		{
			return type;
		}

		public Object getData()
		{
			return data;
		}
	}
}
