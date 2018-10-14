using System;
using com.tvd12.ezyfoxserver.client.constant;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySimplePacket : EzyPacket
	{
		private Object data;
		private EzyTransportType transportType = EzyTransportType.TCP;

		public void setData(Object data)
		{
			this.data = data;
		}

		public object getData()
		{
			return data;
		}

		public int getSize()
		{
			if (data is String)
				return ((String)data).Length;
			return ((byte[])data).Length;
		}

		public EzyTransportType getTransportType()
		{
			return transportType;
		}

		public void setTransportType(EzyTransportType transportType)
		{
			this.transportType = transportType;
		}

		public void release()
		{
			this.data = null;
		}
	}
}
