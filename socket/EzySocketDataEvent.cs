using System;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySocketDataEvent : EzySocketEvent
	{
		protected readonly EzyArray data;

		public EzySocketDataEvent(Object data) : this((EzyArray)data)
		{
		}

		public EzySocketDataEvent(EzyArray data)
		{
			this.data = data;
		}

		public EzyArray getData()
		{
			return data;
		}
	}
}
