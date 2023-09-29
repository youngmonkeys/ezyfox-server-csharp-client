using System;
using AOT;
using com.tvd12.ezyfoxserver.client.logger;
using com.tvd12.ezyfoxserver.client.socket;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public class EzyWSPingSchedule : EzyPingSchedule
	{
		private static readonly EzyLogger LOGGER = EzyUnityLoggerFactory.getLogger(typeof(EzyWSClient));
		
		public EzyWSPingSchedule(EzyClient client) : base(client)
		{
		}

		public override void start()
		{
			EzyWSProxy.run3(
				client.getName(),
				"startPing",
				startPingCallback
			);
		}
		
		[MonoPInvokeCallback(typeof(EzyDelegates.Delegate2))]
		public static void startPingCallback(
			String clientName,
			String jsonData
		)
		{
			LOGGER.debug(
				"startPingCallback: clientName = " +
				clientName + ", jsonData = " + jsonData
			);
		}

		public override void stop()
		{
			EzyWSProxy.run3(
				client.getName(),
				"stopPing",
				stopPingCallback
			);
		}
		
		[MonoPInvokeCallback(typeof(EzyDelegates.Delegate2))]
		public static void stopPingCallback(
			String clientName,
			String jsonData
		)
		{
			LOGGER.debug(
				"stopPingCallback: clientName = " +
				clientName + ", jsonData = " + jsonData
			);
		}
		
		public override void setSocketEventQueue(EzySocketEventQueue socketEventQueue)
		{
			throw new InvalidOperationException("not supported");
		}
	}
}
