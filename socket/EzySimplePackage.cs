using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySimplePackage : EzyPackage
	{
		protected EzyArray data;
		protected EzyTransportType transportType;

		public EzySimplePackage(EzyArray data) : this(data, EzyTransportType.TCP)
		{
		}

		public EzySimplePackage(EzyArray data, EzyTransportType transportType)
		{
			this.data = data;
			this.transportType = transportType;
		}

		public EzyArray getData()
		{
			return data;
		}

		public EzyTransportType getTransportType()
		{
			return transportType;
		}

		public void release()
		{
			this.data = null;
		}

	}
}
