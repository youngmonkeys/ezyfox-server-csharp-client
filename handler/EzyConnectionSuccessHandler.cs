using System;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.security;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyConnectionSuccessHandler : EzyAbstractEventHandler<EzyEvent>
	{
		protected override sealed void process(EzyEvent evt)
		{
            client.setStatus(EzyConnectionStatus.CONNECTED);
			sendHandshakeRequest();
			postHandle();
		}

		protected virtual void postHandle()
		{
		}

		protected void sendHandshakeRequest()
		{
			EzyRequest request = newHandshakeRequest();
			client.send(request);
		}

		protected EzyRequest newHandshakeRequest()
		{
			return new EzyHandshakeRequest(
				   getClientId(),
                   generateClientKey(),
				   "CSHARP",
				   "1.0.0",
				   client.isEnableSSL(),
				   getStoredToken()
		   );
		}

        protected virtual String getClientId()
        {
            Guid guid = Guid.NewGuid();
            String id = guid.ToString();
            return id;
        }

        protected byte[] generateClientKey()
        {
            if (!client.isEnableSSL())
            {
                return null;
            }
            EzyKeyPair keyPair = EzyKeyPairGentor.builder()
                .build()
                .generate();
            byte[] publicKey = keyPair.getPublicKey();
            byte[] privateKey = keyPair.getPrivateKey();
            client.setPublicKey(publicKey);
            client.setPrivateKey(privateKey);
            return publicKey;
        }

        protected virtual String getStoredToken()
		{
			return "";
		}
	}

}
