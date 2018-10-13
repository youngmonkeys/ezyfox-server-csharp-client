using System;

namespace com.tvd12.ezyfoxserver.client.util
{
	public interface EzyRoProperties
	{
		Properties getProperties();

		T getProperty<T>();

		T getProperty<T>(Object key);

		bool containsKey(Object key);
	}
}
