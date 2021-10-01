using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client.util
{
	public sealed class EzyObjectToMap
	{

		private static readonly EzyObjectToMap INSTANCE = new EzyObjectToMap();

		public static EzyObjectToMap getInstance()
		{
			return INSTANCE;
		}

		public IDictionary<K, V> toMap<K, V>(EzyObject obj)
		{
			IDictionary<K, V> answer = new Dictionary<K, V>();
			foreach (Object key in obj.keys())
			{
				Object value = obj.get<Object>(key);
				Object skey = key;
				EzyArrayToList arrayToList = EzyArrayToList.getInstance();
				if (key is EzyArray) {
					skey = arrayToList.toList<object>((EzyArray)key);
				}
				else if (key is EzyObject) {
					skey = toMap<object, object>((EzyObject)key);
				}
				Object svalue = value;
				if (value != null)
				{
					if (value is EzyArray) {
						svalue = arrayToList.toList<object>((EzyArray)value);
					}
					if (value is EzyObject) {
						svalue = toMap<object, object>((EzyObject)value);
					}
			}
			answer[(K)skey] = (V)svalue;
		}
		return answer;
	}

}

}
