using System;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;

namespace com.tvd12.ezyfoxserver.client.request
{
	public class EzyPingRequest : EzyRequest
	{
		public Object getCommand()
		{
			return EzyCommand.PING;
		}

		public EzyData serialize()
		{
			return EzyEntityFactory.EMPTY_ARRAY;
		}
	}
}
