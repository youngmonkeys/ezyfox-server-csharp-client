using System;
using System.Collections;
using com.tvd12.ezyfoxserver.client.entity;
using com.tvd12.ezyfoxserver.client.factory;

namespace com.tvd12.ezyfoxserver.client.io
{
	public class EzyInputTransformer
	{
		public Object transform(Object value)
		{
			return value == null
					? transformNullValue(value)
					: transformNonNullValue(value);
		}

		protected Object transformNullValue(Object value)
		{
			return null;
		}

		protected Object transformNonNullValue(Object value)
		{
			if (value is IEnumerable)
			{
				IEnumerable collection = (IEnumerable)value;
				EzyArray array = EzyEntityFactory.newArray();
				foreach (Object item in collection)
				{
					array.add(transform(item));
				}
				return array;
			}
			if (value is IDictionary) {
				IDictionary dictionary = (IDictionary)value;
				EzyObject obj = EzyEntityFactory.newObject();
				foreach (DictionaryEntry entry in dictionary)
				{
					obj.put(transform(entry.Key), transform(entry.Value));
				}
				return obj;
			}
			return value;
		}
	}
}