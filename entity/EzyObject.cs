using System;
using System.Text;
using System.Collections.Generic;

namespace com.tvd12.ezyfoxserver.client.entity
{
	public class EzyObject : EzyRoObject
	{
		protected readonly Dictionary<Object, Object> dictionary 
				= new Dictionary<Object, Object>();

		public void clear()
		{
			dictionary.Clear();
		}

		public void put(Object key, Object value)
		{
			dictionary[key] = value;
		}

		public void putAll<K,V>(IDictionary<K, V> dict)
		{
			foreach (K key in dict.Keys)
			{
				dictionary[key] = dict[key];		
			}
		}

		public int size()
		{
			return dictionary.Count;
		}

		public bool isEmpty()
		{
			return size() == 0;
		}

		public bool containsKey(Object key)
		{
			return dictionary.ContainsKey(key);
		}

		public bool isNotNullValue(Object key)
		{
			return dictionary[key] != null;
		}

		public V get<V>(Object key)
		{
			return (V)dictionary[key];
		}

		public V get<V>(Object key, V defValue)
		{
			return containsKey(key) ? get<V>(key) : defValue;
		}

		public ICollection<Object> keys()
		{
			return dictionary.Keys;
		}

		public ICollection<Object> values()
		{
			return dictionary.Values;
		}

		public IDictionary<Object, Object> toDict()
		{
			return dictionary;	
		}

		public object Clone()
		{
			var answer = new EzyObject();
			foreach (Object key in dictionary.Keys)
			{
				Object value = dictionary[key];
				Object ckey = key;
				Object cvalue = value;
				if (key is ICloneable)
				{
					ckey = ((ICloneable)key).Clone();
				}
				if (value is ICloneable)
				{
					cvalue = ((ICloneable)value).Clone();
				}
				answer.put(ckey, cvalue);
			}
			return answer;
		}

		public EzyObject duplicate<EzyObject>()
		{
			return (EzyObject)Clone();
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append("{");
			var count = 0;
			var commable = size() - 1;
			foreach (Object key in dictionary.Keys)
			{
				builder
					.Append(key)
					.Append(":")
					.Append(dictionary[key]);
				if ((count++) < commable)
				{
					builder.Append(",");
				}
			}
			builder.Append("}");
			return builder.ToString();
		}
	}
}
