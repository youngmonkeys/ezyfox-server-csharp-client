using System;
using System.Text;
using com.tvd12.ezyfoxserver.client.manager;

namespace com.tvd12.ezyfoxserver.client.entity
{
    public class EzySimpleZone : EzyEntity, EzyZone
	{
		protected readonly int id;
		protected readonly String name;
		protected readonly EzyClient client;
		protected readonly EzyAppManager appManager;

		public EzySimpleZone(EzyClient client, int id, String name)
		{
			this.id = id;
			this.name = name;
			this.client = client;
			this.appManager = new EzySimpleAppManager(name);
		}

		public int getId()
		{
			return id;
		}

		public String getName()
		{
			return name;
		}

		public EzyClient getClient()
		{
			return client;
		}

		public EzyAppManager getAppManager()
		{
			return appManager;
		}

        public EzyApp getApp()
        {
            return appManager.getApp();
        }

        public override string ToString()
        {
            return new StringBuilder()
                .Append("Zone(")
                .Append("id: ").Append(id).Append(", ")
                .Append("name: ").Append(name)
                .Append(")")
                .ToString();
        }
    }
}