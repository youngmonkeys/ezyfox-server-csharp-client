using System;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.request;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyConnectionSuccessHandler : EzyAbstractEventHandler<EzyEvent>
	{
		protected override sealed void process(EzyEvent evt)
		{
			updateConnectionStatus();
			sendHandshakeRequest();
			postHandle();
		}

		private void updateConnectionStatus()
		{
			client.setStatus(EzyConnectionStatus.CONNECTED);
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
			EzyHandshakeRequest request = new EzyHandshakeRequest(
				   getClientId(),
				   getClientKey(),
				   "CSHARP",
				   "1.0.0",
				   isEnableEncryption(),
				   getStoredToken()
		   );
			return request;
		}

		protected virtual String getClientId()
		{
			Guid guid = Guid.NewGuid();
			String id = guid.ToString();
			return id;
		}

		protected virtual String getClientKey()
		{
			String key = "";
			return key;
		}

		protected virtual bool isEnableEncryption()
		{
			return false;
		}

		protected virtual String getStoredToken()
		{
			return "";
		}
	}

}
