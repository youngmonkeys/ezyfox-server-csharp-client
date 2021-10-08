using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.entity
{
	public class EzyObject : EzyRoObject
	{
		protected readonly Dictionary<Object, Object> dictionary;
		protected readonly EzyInputTransformer inputTransformer;
		protected readonly EzyOutputTransformer outputTransformer;

		public EzyObject(
			EzyInputTransformer inputTransformer,
			EzyOutputTransformer outputTransformer)
		{
			this.inputTransformer = inputTransformer;
			this.outputTransformer = outputTransformer;
			this.dictionary = new Dictionary<Object, Object>();
		}

		public void clear()
		{
			dictionary.Clear();
		}

		public void put(Object key, Object value)
		{
			dictionary[inputTransformer.transform(key)]
				= inputTransformer.transform(value);
		}

		public void putRawDict(IDictionary dict)
		{
			foreach (Object key in dict.Keys)
			{
				dictionary[inputTransformer.transform(key)]
					= inputTransformer.transform(dict[key]);
			}
		}

		public void putAll<K,V>(IDictionary<K, V> dict)
		{
			foreach (K key in dict.Keys)
			{
				dictionary[inputTransformer.transform(key)]
					= inputTransformer.transform(dict[key]);		
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
			return dictionary.ContainsKey(key) && dictionary[key] != null;
		}

		public V get<V>(Object key)
		{
			return dictionary.ContainsKey(key)
				? outputTransformer.transform<V>(dictionary[key])
				: default(V);
		}

		public V get<V>(Object key, V defValue)
		{
			return containsKey(key) ? get<V>(key) : defValue;
		}

		public Object getByOutType(Object key, Type outType)
		{
			return dictionary.ContainsKey(key)
				? outputTransformer.transformByOutType(dictionary[key], outType)
				: null;
		}

		public ICollection<Object> keys()
		{
			return dictionary.Keys;
		}

		public ICollection<Object> values()
		{
			return dictionary.Values;
		}

		public Dictionary<K, V> toDict<K, V>()
		{
			EzyObjectToMap objectToMap = EzyObjectToMap.getInstance();
			Dictionary<K, V> map = objectToMap.toMap<K, V>(this);
			return map;
		}

		public object Clone()
		{
			var answer = new EzyObject(inputTransformer, outputTransformer);
			foreach (Object key in dictionary.Keys)
			{
				Object value = dictionary[key];
				Object ckey = key;
				Object cvalue = value;
				if (key is ICloneable)
				{
					ckey = ((ICloneable)key).Clone();
				}
				if (value != null && value is ICloneable)
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
