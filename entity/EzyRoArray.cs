using System;
using System.Collections.Generic;

namespace com.tvd12.ezyfoxserver.client.entity
{
	public interface EzyRoArray : EzyData
	{
		int size();

		bool isEmpty();

		T get<T>(int index);

		T get<T>(int index, T defValue);

		Object getByOutType(int index, Type outType);

		List<T> toList<T>();
	}
}
