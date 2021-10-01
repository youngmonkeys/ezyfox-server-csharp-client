using System;
using System.Collections.Generic;

namespace com.tvd12.ezyfoxserver.client.entity
{
	public interface EzyRoObject : EzyData
	{
		int size();
		bool isEmpty();
		bool containsKey(Object key);
		bool isNotNullValue(Object key);
		V get<V>(Object key);
		V get<V>(Object key, V defValue);
		ICollection<Object> keys();
		ICollection<Object> values();
		IDictionary<K, V> toDict<K, V>();
	}
}
