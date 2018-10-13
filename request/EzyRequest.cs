using System;
using com.tvd12.ezyfoxserver.client.util;
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.request
{

	public interface EzyRequest
	{
		Object getCommand();

		EzyData serialize();
	}
}
