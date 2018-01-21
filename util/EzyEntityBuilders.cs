using System;
using com.tvd12.ezyfoxserver.client.builder;
using com.tvd12.ezyfoxserver.client.factory;
namespace com.tvd12.ezyfoxserver.client.util
{
	public class EzyEntityBuilders : EzyLoggable
	{
		protected EzyArrayBuilder newArrayBuilder()
		{
			return EzyEntityFactory.create<EzyArrayBuilder>();
		}

		protected EzyObjectBuilder newObjectBuilder()
		{
			return EzyEntityFactory.create<EzyObjectBuilder>();
		}
	}
}
