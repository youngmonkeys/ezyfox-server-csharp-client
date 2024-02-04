using System.Collections.Generic;

namespace com.tvd12.ezyfoxserver.client.util
{
	public sealed class EzyDictionaries
	{
		private EzyDictionaries()
        {
        }

		public static V getOrDefault<K, V>(
			IDictionary<K, V> dict,
			K key,
			V defaultValue
		)
        {
			return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }
	}
}
