using com.tvd12.ezyfoxserver.client.constant;

namespace com.tvd12.ezyfoxserver.client.evt
{
	public class EzyConnectionFailureEvent : EzyEvent
	{
		private readonly EzyConnectionFailedReason reason;

		private EzyConnectionFailureEvent(EzyConnectionFailedReason reason)
		{
			this.reason = reason;
		}

		public static EzyConnectionFailureEvent timeout()
		{
			return new EzyConnectionFailureEvent(EzyConnectionFailedReason.TIME_OUT);
		}

		public static EzyConnectionFailureEvent networkUnreachable()
		{
			return new EzyConnectionFailureEvent(EzyConnectionFailedReason.NETWORK_UNREACHABLE);
		}

		public static EzyConnectionFailureEvent unknownHost()
		{
			return new EzyConnectionFailureEvent(EzyConnectionFailedReason.UNKNOWN_HOST);
		}

		public static EzyConnectionFailureEvent connectionRefused()
		{
			return new EzyConnectionFailureEvent(EzyConnectionFailedReason.CONNECTION_REFUSED);
		}

		public static EzyConnectionFailureEvent unknown()
		{
			return new EzyConnectionFailureEvent(EzyConnectionFailedReason.UNKNOWN);
		}

		public EzyConnectionFailedReason getReason()
		{
			return reason;
		}

		public EzyEventType getType()
		{
			return EzyEventType.CONNECTION_FAILURE;
		}
	}
}
