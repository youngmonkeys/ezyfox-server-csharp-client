using System;

namespace com.tvd12.ezyfoxserver.client.entity
{
    public class EzySimpleUser : EzyEntity, EzyUser
	{
		protected readonly long id;
		protected readonly String name;

		public EzySimpleUser(long id, String name)
		{
			this.id = id;
			this.name = name;
		}

		public long getId()
		{
			return id;
		}

		public String getName()
		{
			return name;
		}
	}
}
