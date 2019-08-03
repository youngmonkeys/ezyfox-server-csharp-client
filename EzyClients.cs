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
            lock (clients)
            {
                return newClient0(config);
            }
		}

        private EzyClient newClient0(EzyClientConfig config)
        {
            String clientName = config.getClientName();
            if (clients.ContainsKey(clientName))
                return clients[clientName];
            EzyClient client = new EzyTcpClient(config);
            addClient0(client);
            if (defaultClientName == null)
                defaultClientName = client.getName();
            return client;
        }

        public EzyClient newDefaultClient(EzyClientConfig config)
        {
            lock (clients)
            {
                EzyClient client = newClient0(config);
                defaultClientName = client.getName();
                return client;
            }
		}

		public void addClient(EzyClient client)
		{
            lock(clients)
            {
                addClient0(client);
            }
		}

        private void addClient0(EzyClient client)
        {
            this.clients[client.getName()] = client;
        }

		public EzyClient getClient(Object name)
		{
            lock (clients)
            {
                return getClient0(name);
            }
		}

        private EzyClient getClient0(Object name)
        {
            if (name == null)
                throw new ArgumentException("can not get client with name: null");
            if (clients.ContainsKey(name))
                return clients[name];
            return null;
        }

		public EzyClient getDefaultClient()
		{
			EzyClient client = getClient(defaultClientName);
			return client;
		}

        public void getClients(IList<EzyClient> cachedClients) 
        {
            cachedClients.Clear();
            lock(clients)
            {
                foreach (EzyClient client in clients.Values)
                    cachedClients.Add(client);
            }
        }

	}
}
