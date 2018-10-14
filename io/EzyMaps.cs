using System;
using System.Collections.Generic;

namespace com.tvd12.ezyfoxserver.client.io
{
	public sealed class EzyMaps
	{
		private EzyMaps()
		{
		}

		public static IList<V> values<K, V>(IDictionary<K, V> map)
		{
			var answer = new List<V>();
			foreach (var pair in map)
			{
				answer.Add(pair.Value);
			}
			return answer;
		}

		public static ISet<K> keySet<K,V>(IDictionary<K, V> map)
		{
			var answer = new HashSet<K>();
			foreach (var pair in map)
			{
				answer.Add(pair.Key);
			}
			return answer;
		}

		public static IDictionary<K, V> clone<K, V>(IDictionary<K, V> map)
		{
			var answer = new Dictionary<K, V>();
			putAll(answer, map);
			return answer;
		}

		public static void putAll<K, V>(IDictionary<K, V> a, IDictionary<K, V> b)
		{
			foreach (var pair in b)
			{
				a[pair.Key] = pair.Value;
			}
		}
	}
}
