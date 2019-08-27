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
            String reasonName = EzyDisconnectReasons.getDisconnectReasonName(evt.getReason());
            logger.info("handle disconnection, reason = " + reasonName);
			preHandle(evt);
			EzyClientConfig config = client.getConfig();
			EzyReconnectConfig reconnectConfig = config.getReconnect();
			bool should = shouldReconnect(evt);
			bool mustReconnect = reconnectConfig.isEnable() && should;
			bool reconnecting = false;
            client.setStatus(EzyConnectionStatus.DISCONNECTED);
			if (mustReconnect)
				reconnecting = client.reconnect();
			if (!reconnecting)
			{
				control(evt);
			}
		}

        protected virtual void preHandle(EzyDisconnectionEvent evt)
		{
		}

        protected virtual bool shouldReconnect(EzyDisconnectionEvent evt)
		{
			return true;
		}

        protected virtual void control(EzyDisconnectionEvent evt)
		{
		}
	}
}
