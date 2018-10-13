using System;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public interface IEzyAppDataHandler
	{
	}

	public interface EzyAppDataHandler<D> : IEzyAppDataHandler where D : EzyData
	{
		void handle(EzyApp app, D data);
	}
}
