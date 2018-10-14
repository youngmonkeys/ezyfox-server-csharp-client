using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.request;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public interface EzySender
	{
		void send(EzyRequest request);

		void send(Object cmd, EzyData data);
	}
}
