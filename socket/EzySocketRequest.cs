using System;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public interface EzySocketRequest
	{
		EzyArray getData();

		long getTimestamp();

		int getCommand();

	}
}
