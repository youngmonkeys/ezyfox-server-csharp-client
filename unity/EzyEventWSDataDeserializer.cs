using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.constant;
using com.tvd12.ezyfoxserver.client.evt;
using Newtonsoft.Json;

namespace com.tvd12.ezyfoxserver.client.unity
{
	public sealed class EzyEventWSDataDeserializer
	{
		private static readonly Dictionary<String, EzyEventType>
			EVENT_TYPE_BY_STRING_VALUE = new()
			{
				{ "CONNECTION_SUCCESS", EzyEventType.CONNECTION_SUCCESS },
				{ "CONNECTION_FAILURE", EzyEventType.CONNECTION_FAILURE },
				{ "DISCONNECTION", EzyEventType.DISCONNECTION },
				{ "LOST_PING", EzyEventType.LOST_PING },
				{ "TRY_CONNECT", EzyEventType.TRY_CONNECT },
			};

		private static readonly Dictionary<String, EzyConnectionFailedReason>
			CONNECTION_FAILED_REASON_BY_STRING_VALUE = new()
			{
				{ "TIME_OUT", EzyConnectionFailedReason.TIME_OUT },
				{ "NETWORK_UNREACHABLE", EzyConnectionFailedReason.NETWORK_UNREACHABLE },
				{ "UNKNOWN_HOST", EzyConnectionFailedReason.UNKNOWN_HOST },
				{ "CONNECTION_REFUSED", EzyConnectionFailedReason.CONNECTION_REFUSED },
				{ "UNKNOWN", EzyConnectionFailedReason.UNKNOWN },
			};

		private static readonly Dictionary<EzyEventType, EventDeserializer>
			DESERIALIZER_BY_EVENT_TYPE = new()
			{
				{ EzyEventType.CONNECTION_SUCCESS, _ => new EzyConnectionSuccessEvent() },
				{
					EzyEventType.CONNECTION_FAILURE, jsonData => new EzyConnectionFailureEvent(
						CONNECTION_FAILED_REASON_BY_STRING_VALUE[
							JsonConvert.DeserializeObject<Dictionary<String, String>>(jsonData)
								.GetValueOrDefault("reason", "UNKNOWN")
						]
					)
				},
				{
					EzyEventType.DISCONNECTION, jsonData => new EzyDisconnectionEvent(
						JsonConvert.DeserializeObject<Dictionary<String, int>>(jsonData)
							.GetValueOrDefault("reason", 0)
					)
				},
				{
					EzyEventType.LOST_PING, jsonData => new EzyLostPingEvent(
						JsonConvert.DeserializeObject<Dictionary<String, int>>(jsonData)
							.GetValueOrDefault("count", 0)
					)
				},
				{
					EzyEventType.TRY_CONNECT, jsonData => new EzyTryConnectEvent(
						JsonConvert.DeserializeObject<Dictionary<String, int>>(jsonData)
							.GetValueOrDefault("count", 0)
					)
				}
			};

		private delegate EzyEvent EventDeserializer(String jsonData);

		private static readonly EzyEventWSDataDeserializer INSTANCE = new();

		public static EzyEventWSDataDeserializer getInstance()
		{
			return INSTANCE;
		}

		public EzyEvent deserializeEvent(
			String eventTypeStringValue,
			String jsonData
		)
		{
			var eventType = EVENT_TYPE_BY_STRING_VALUE[eventTypeStringValue];
			return DESERIALIZER_BY_EVENT_TYPE[eventType].Invoke(jsonData);
		}
	}
}
