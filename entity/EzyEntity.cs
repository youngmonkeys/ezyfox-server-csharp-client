using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.entity
{
	public class EzyEntity : EzyProperties
	{
		protected Properties properties = new Properties();

		public void setProperty(Object key, Object value)
		{
			properties.set(key, value);
		}

		public void setProperties(IDictionary<Object, Object> dict)
		{
			properties.putAll(dict);
		}

		public T getProperty<T>()
		{
			Type type = typeof(T);
			if (properties.containsKey(type))
			{
				T answer = (T)properties.get(type);
				return answer;
			}
			return default(T);
		}

		public T getProperty<T>(Object key)
		{
			T t = (T)properties.get(key);
			return t;
		}

		public void removeProperty(Object key)
		{
			properties.remove(key);
		}

		public bool containsKey(Object key)
		{
			bool answer = properties.containsKey(key);
			return answer;
		}

		public Properties getProperties()
		{
			Properties props = new Properties();
			props.putAll(properties);
			return props;
		}

	}
}
