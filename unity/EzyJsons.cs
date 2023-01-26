using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Object = System.Object;

namespace com.tvd12.ezyfoxserver.client.util
{
	public static class EzyJsons
	{
		public static String serialize(Object obj)
		{
			switch (obj)
			{
				case EzyObject ezyObject:
					return JsonConvert.SerializeObject(ezyObject.toDict<string, object>());
				case EzyArray ezyArray:
					return JsonConvert.SerializeObject(ezyArray.toList<object>());
				default:
					return JsonConvert.SerializeObject(obj);
			}	
		}
		
		public static EzyData deserialize(string json)
		{
			Object obj = parseJElement(JsonConvert.DeserializeObject(json));
			switch (obj)
			{
				case IList list:
					return EzyEntityFactory.newArrayBuilder()
						.appendRawList(list)
						.build();
				case IDictionary dict:
					return EzyEntityFactory.newObjectBuilder()
						.appendRawDict(dict)
						.build();
				default:
					throw new Exception($"Failed to deserialize json to EzyData");
			}
		}

		private static object parseJElement(object jElement)
		{
			switch (jElement)
			{
				case JObject jObject: // jObject becomes Dictionary<string, object>
					return ((IEnumerable<KeyValuePair<string, JToken>>)jObject).ToDictionary(
						j => j.Key,
						j => parseJElement(j.Value)
					);
				case JArray jArray: // jArray becomes List<object>
					return jArray.Select(parseJElement).ToList();
				case JValue jValue: // jValue just becomes the value
					return jValue.Value;
				default:
					throw new Exception($"Unsupported type: {jElement.GetType()}");
			}
		}
	}
}
