using System;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.evt
{
	public class EzyEventFactory
	{
		public EzyHandshakeEvent newHandshakeEvent(EzyArray input)
		{
			var args = new EzyHandshakeEventArgs();
			args.deserialize(input);
			return new EzyHandshakeEvent(args);
		}

		public EzyLoginSuccessEvent newLoginSuccessEvent(EzyArray input)
		{
			var args = new EzyLoginSuccessEventArgs();
			args.deserialize(input);
			return new EzyLoginSuccessEvent(args);
		}

		public EzyConnectionSuccessEvent newConnectionSuccessEvent()
		{
			return new EzyConnectionSuccessEvent();
		}
	}
}
