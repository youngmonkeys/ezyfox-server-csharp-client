using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.request;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyConnectionSuccessHandler : EzyAbstractEventHandler<EzyEvent>
	{

		public override void handle(EzyEvent evt)
		{
			updateConnectionStatus();
			sendHandshakeRequest();
			postHandle();
		}

		private void updateConnectionStatus()
		{
			client.setStatus(EzyConnectionStatus.CONNECTED);
		}

		protected void postHandle()
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
					UUID.randomUUID().toString(),
					"",
					"ANDROID",
					"1.0.0",
					false,
					""
			);
			return request;
		}
	}

}
