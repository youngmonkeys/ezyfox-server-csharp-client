using System;
using System.Text;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.request;
using com.tvd12.ezyfoxserver.client.security;
using com.tvd12.ezyfoxserver.client.socket;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public abstract class EzyHandshakeHandler :
		EzyAbstractDataHandler,
		EzyPingScheduleAware
	{
		protected EzyPingSchedule pingSchedule;

		public override void handle(EzyArray data)
		{
            preHandle(data);
			pingSchedule.start();
			handleLogin(data);
			postHandle(data);
		}

        protected void preHandle(EzyArray data)
        {
            this.client.setSessionToken(data.get<String>(1));
            this.client.setSessionId(data.get<long>(2));
            if (client.isEnableSSL())
            {
                this.client.setSessionKey(
                    decryptSessionKey(data.get<byte[]>(3, null))
				);
			}
		}

        protected byte[] decryptSessionKey(byte[] sessionKey)
        {
            if (sessionKey == null)
            {
                if (client.isEnableDebug())
                {
                    return null;
                }
                this.client.close();
                throw new Exception(
                    "maybe server was not enable SSL, you must enable SSL on server " +
                        "or disable SSL on your client or enable debug mode"
                );
            }
            try
            {
                return EzyAsyCrypt.builder()
                    .privateKey(client.getPrivateKey())
                    .build()
                    .decrypt(sessionKey);
            }
            catch (Exception e)
            {
                throw new Exception(
                    "can not decrypt session key: " + Encoding.UTF8.GetString(sessionKey),
                    e
                );
            }
        }

        protected virtual void postHandle(EzyArray data)
		{
		}

		protected void handleLogin(EzyArray data)
		{
			EzyRequest loginRequest = getLoginRequest();
			client.send(loginRequest);
		}

		protected abstract EzyRequest getLoginRequest();

		public void setPingSchedule(EzyPingSchedule pingSchedule)
		{
			this.pingSchedule = pingSchedule;
		}
	}
}
