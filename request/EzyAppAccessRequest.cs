using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.factory;

namespace com.tvd12.ezyfoxserver.client.request
{
	public class EzyAppAccessRequest : EzyRequest
	{
		protected readonly String appName;
		protected readonly EzyData data;

		public EzyAppAccessRequest(String appName) : this(appName, null)
		{	
		}

		public EzyAppAccessRequest(String appName, EzyData data)
		{
			this.appName = appName;
			this.data = data;
		}

		public EzyData serialize()
		{
			EzyData answer = EzyEntityFactory.newArrayBuilder()
					.append(appName)
					.append(data)
					.build();
			return answer;
		}

		public Object getCommand()
		{
			return EzyCommand.APP_ACCESS;
		}
	}

}
