using System;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public interface EzyResponse : EzyReleasable
	{
		EzyArray getData();

		DateTime getTimestamp();

		EzyCommand getCommand();
	}
}
