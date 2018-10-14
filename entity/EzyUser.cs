using System;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.entity
{
	public interface EzyUser : EzyDestroyable
	{
		long getId();

		String getName();
	}
}
