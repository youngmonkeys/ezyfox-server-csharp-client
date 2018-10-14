using System;
namespace com.tvd12.ezyfoxserver.client.util
{
	public interface EzyInstanceFetcher
	{
		 T get<T>();
	}
}
