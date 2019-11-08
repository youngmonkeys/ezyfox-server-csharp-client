using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.factory;

namespace com.tvd12.ezyfoxserver.client.request
{
	public class EzyAppExitRequest : EzyRequest
	{
		protected readonly int appId;

        public EzyAppExitRequest(int appId)
		{
            this.appId = appId;
		}

		public EzyData serialize()
		{
			EzyData answer = EzyEntityFactory.newArrayBuilder()
                    .append(appId)
                    .build();
			return answer;
		}

		public Object getCommand()
		{
            return EzyCommand.APP_EXIT;
		}
	}

}
