using System;
using System.Collections.Generic;

namespace com.tvd12.ezyfoxserver.client.util
{
	public class Properties
	{
		private readonly IDictionary<Object, Object> holder;

		public Properties()
		{
			this.holder = new Dictionary<Object, Object>();
		}

		public Object get(Object key)
		{
			Object answer = null;
			if (holder.ContainsKey(key))
				answer = holder[key];
			return answer;
		}

		public Object put(Object key, Object value)
		{
			Object old = null;
			if (holder.ContainsKey(key))
				old = holder[key];
			holder[key] = value;
			return old;
		}

		public void set(Object key, Object value)
		{
			holder[key] = value;
		}

		public void putAll(Properties props)
		{
			foreach (var entry in props.holder)
				holder[entry.Key] = entry.Value;
		}

		public void putAll(IDictionary<Object, Object> dict)
		{
			foreach (var entry in dict)
				holder[entry.Key] = entry.Value;
		}

		public void remove(Object key)
		{
			holder.Remove(key);
		}

		public bool containsKey(Object key)
		{
			bool answer = holder.ContainsKey(key);
			return answer;
		}

		public int size()
		{
			int size = holder.Count;
			return size;
		}

		public void clear()
		{
			holder.Clear();
		}
	}
}
