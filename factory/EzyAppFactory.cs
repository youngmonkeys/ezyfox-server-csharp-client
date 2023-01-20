using System;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.factory
{
	public class EzyAppFactory
	{
		public virtual EzyApp newApp(
			EzyZone zone,
			int id,
			String name
		)
        {
			return new EzySimpleApp(zone, id, name);
        }
	}
}
