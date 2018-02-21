using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.handler;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.security;

namespace com.tvd12.ezyfoxserver.client.api
{
	public class EzyConnectionSuccessEventHandler :
			EzyAbstractEventHandler<EzyConnectionSuccessEvent>,
			EzyRequestDeliverAware
	{
		protected int keySize;
		protected EzyKeyPairGentor keyPairGentor;
		protected EzyRequestDeliver requestDeliver;
		protected EzyClientIdFetcher clientIdFetcher;

		public override void handle(EzyConnectionSuccessEvent evt)
		{
			prehandle();
			handle0(evt);
		}

		protected void prehandle()
		{
			if (keySize == 0)
			{
				this.keySize = defaultKeySize();
			}
			if (keyPairGentor == null)
			{
				this.keyPairGentor = defaultKeyPairGentor();
			}
			if (clientIdFetcher == null)
			{
				this.clientIdFetcher = defaultClientIdFetcher();
			}
		}

		protected void handle0(EzyConnectionSuccessEvent evt)
		{
			var keyPair = generateKeyPair();
			var clientKey = keyPair.getPublicKey();
			getLogger().info("public key: {0}", clientKey);
			var clientId = clientIdFetcher.getClientId();
			var parameters = new EzyHandshakeRequestParams();
			parameters.setClientId(clientId);
			parameters.setClientKey(clientKey);
			parameters.setReconnectToken("reconnectToken");
			requestDeliver.send(new EzyHandshakeRequest(parameters));
		}

		protected EzyKeyPair generateKeyPair()
		{
			return keyPairGentor.generate(keySize);
		}

		protected void sendHandShakeRequest()
		{
			var parameters = new EzyHandshakeRequestParams();
			var request = new EzyHandshakeRequest(parameters);
			requestDeliver.send(request);
		}

		public void setRequestDeliver(EzyRequestDeliver deliver)
		{
			this.requestDeliver = deliver;
		}

		protected int defaultKeySize()
		{
			return 512;
		}

		protected virtual EzyKeyPairGentor defaultKeyPairGentor()
		{
			return new EzyRsaKeyPairGentor();
		}

		public virtual EzyClientIdFetcher defaultClientIdFetcher()
		{
			return new EzyUnknownClientIdFetcher();
		}

	}
}
