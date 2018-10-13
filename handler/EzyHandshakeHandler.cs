using System;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.evt;
using com.tvd12.ezyfoxserver.client.handler;
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
			pingSchedule.start();
			handleLogin(data);
			postHandle(data);
		}

		protected void postHandle(EzyArray data)
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
