using System;
using com.tvd12.ezyfoxserver.client.config;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.evt;

namespace com.tvd12.ezyfoxserver.client.handler
{
	public class EzyDisconnectionHandler : EzyAbstractEventHandler<EzyDisconnectionEvent>
	{
		protected override void process(EzyDisconnectionEvent evt)
		{
			Console.WriteLine("handle disconnection, reason = " + evt.getReason());
			preHandle(evt);
			EzyClientConfig config = client.getConfig();
			EzyReconnectConfig reconnectConfig = config.getReconnect();
			bool should = shouldReconnect(evt);
			bool mustReconnect = reconnectConfig.isEnable() && should;
			bool reconnecting = false;
			if (mustReconnect)
				reconnecting = client.reconnect();
			if (!reconnecting)
			{
				client.setStatus(EzyConnectionStatus.DISCONNECTED);
				control(evt);
			}
		}

		protected void preHandle(EzyDisconnectionEvent evt)
		{
		}

		protected bool shouldReconnect(EzyDisconnectionEvent evt)
		{
			return true;
		}

		protected void control(EzyDisconnectionEvent evt)
		{
		}
	}
}
