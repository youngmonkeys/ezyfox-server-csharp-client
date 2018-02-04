using System;

namespace com.tvd12.ezyfoxserver.client.io
{
	public interface EzyDataDeserializable<T>
	{
		void deserialize(T t);
	}
}
