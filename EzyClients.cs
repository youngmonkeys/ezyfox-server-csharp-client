using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.config;

namespace com.tvd12.ezyfoxserver.client
{
	public sealed class EzyClients
	{

		private String defaultClientName;
		private readonly IDictionary<Object, EzyClient> clients;
		private static readonly EzyClients INSTANCE = new EzyClients();

		private EzyClients()
		{
			this.clients = new Dictionary<Object, EzyClient>();
		}

		public static EzyClients getInstance()
		{
			return INSTANCE;
		}

		public EzyClient newClient(EzyClientConfig config)
		{
			EzyClient client = new EzyTcpClient(config);
			addClient(client);
			if (defaultClientName == null)
				defaultClientName = client.getZoneName();
			return client;
		}

		public EzyClient newDefaultClient(EzyClientConfig config)
		{
			EzyClient client = newClient(config);
			defaultClientName = config.getZoneName();
			return client;
		}

		public void addClient(EzyClient client)
		{
			this.clients[client.getZoneName()] = client;
		}

		public EzyClient getClient(Object name)
		{
			if (clients.ContainsKey(name))
				return clients[name];
			throw new ArgumentException("has no client with name: " + name);
		}

		public EzyClient getDefaultClient()
		{
			EzyClient client = getClient(defaultClientName);
			return client;
		}

	}
}
