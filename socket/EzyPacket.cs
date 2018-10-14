using System;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public interface EzyPacket : EzyReleasable
	{
		Object getData();

		int getSize();

		EzyTransportType getTransportType();
	}

}
