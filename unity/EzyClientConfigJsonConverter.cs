using System;
using com.tvd12.ezyfoxserver.client.config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Extensions.ezyfox_server_csharp_client.unity
{
	public class EzyClientConfigJsonConverter : JsonConverter<EzyClientConfig>
	{
		public override void WriteJson(JsonWriter writer, EzyClientConfig clientConfig, JsonSerializer serializer)
		{
			JObject reconnectConfigJson = new JObject();
			EzyReconnectConfig reconnectConfig = clientConfig?.getReconnect();
			reconnectConfigJson.Add("enable", reconnectConfig?.isEnable());
			reconnectConfigJson.Add("maxReconnectCount", reconnectConfig?.getMaxReconnectCount());
			reconnectConfigJson.Add("reconnectPeriod", reconnectConfig?.getReconnectPeriod());

			JObject pingJson = new JObject();
			EzyPingConfig pingConfig = clientConfig?.getPing();
			pingJson.Add("pingPeriod", pingConfig?.getPingPeriod());
			pingJson.Add("maxLostPingCount", pingConfig?.getMaxLostPingCount());

			JObject clientConfigJson = new JObject();
			clientConfigJson.Add("zoneName", clientConfig?.getZoneName());
			clientConfigJson.Add("clientName", clientConfig?.getClientName());
			clientConfigJson.Add("reconnect", reconnectConfigJson);
			clientConfigJson.Add("ping", pingJson);

			clientConfigJson.WriteTo(writer);
		}

		public override EzyClientConfig? ReadJson(JsonReader reader, Type objectType, EzyClientConfig? existingValue, bool hasExistingValue,
		                                          JsonSerializer serializer)
		{
			throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
		}

		public override bool CanRead => false;
	}
}
