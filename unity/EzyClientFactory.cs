using com.tvd12.ezyfoxserver.client.config;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public sealed class EzyClientFactory
	{
		private static readonly EzyClientFactory INSTANCE = new();

		private EzyClientFactory()
		{
		}

		public static EzyClientFactory getInstance()
		{
			return INSTANCE;
		}

		public EzyClient getOrCreateClient(EzyClientConfig config, bool udpUsage)
		{
			var ezyClients = EzyClients.getInstance();
			var ezyClient = ezyClients.getClient(config.getZoneName());
			if (ezyClient != null)
			{
				return ezyClient;
			}
#if UNITY_WEBGL && !UNITY_EDITOR	
			EzyWSClient ezyWsClient = new EzyWSClient(config);
			ezyClients.addClient(ezyWsClient);
			ezyWsClient.init();
			return ezyWsClient;
#else
			if (udpUsage)
			{
				ezyClient = new EzyUTClient(config);
			}
			else
			{
				ezyClient = new EzyTcpClient(config);
			}
			ezyClients.addClient(ezyClient);
			return ezyClient;
#endif
		}
	}
}
