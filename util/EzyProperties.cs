using System;
using System.Collections.Generic;

namespace com.tvd12.ezyfoxserver.client.util
{
	public interface EzyProperties : EzyRoProperties
	{
		void setProperty(Object key, Object value);

		void setProperties(IDictionary<Object, Object> dict);

		void removeProperty(Object key);
	}

}
