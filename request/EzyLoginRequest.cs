using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.factory;

namespace com.tvd12.ezyfoxserver.client.request
{
	public class EzyLoginRequest : EzyRequest
	{
        protected readonly String zoneName;
        protected readonly String username;
        protected readonly String password;
        protected readonly EzyData data;

		public EzyLoginRequest(String zoneName,
							   String username,
		                       String password) 
			: this(zoneName, username, password, null)
		{
		}

		public EzyLoginRequest(String zoneName,
							   String username,
							   String password,
							   EzyData data)
		{
			this.zoneName = zoneName;
			this.username = username;
			this.password = password;
			this.data = data;
		}

		public Object getCommand()
		{
			return EzyCommand.LOGIN;
		}

		public EzyData serialize()
		{
			EzyData answer = EzyEntityFactory.newArrayBuilder()
					.append(zoneName)
					.append(username)
					.append(password)
					.append(data).build();
			return answer;
		}
	}

}
