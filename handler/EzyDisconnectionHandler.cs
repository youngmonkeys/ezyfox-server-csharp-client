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
			bool mustReconnect = reconnectConfig.isEnable() &&
				evt.getReason() != (int)EzyDisconnectReason.UNAUTHORIZED &&
				evt.getReason() != (int)EzyDisconnectReason.CLOSE &&
				should;
			bool reconnecting = false;
            client.setStatus(EzyConnectionStatus.DISCONNECTED);
			client.setUdpStatus(EzyConnectionStatus.DISCONNECTED);
			if (mustReconnect)
				reconnecting = client.reconnect();
			if (!reconnecting)
			{
				control(evt);
			}
			postHandle(evt);
		}

        protected virtual void preHandle(EzyDisconnectionEvent evt)
		{
		}

		protected virtual void postHandle(EzyDisconnectionEvent evt)
		{
		}

		protected virtual bool shouldReconnect(EzyDisconnectionEvent evt)
		{
            int reason = evt.getReason();
            if (reason == (int)EzyDisconnectReason.ANOTHER_SESSION_LOGIN)
                return false;
			return true;
		}

        protected virtual void control(EzyDisconnectionEvent evt)
		{
		}
	}
}
