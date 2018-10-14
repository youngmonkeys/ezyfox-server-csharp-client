using System;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public interface IEzyAppDataHandler
	{
	}

	public interface EzyAppDataHandler : IEzyAppDataHandler
	{
		void handle(EzyApp app, EzyData data);
	}
}
