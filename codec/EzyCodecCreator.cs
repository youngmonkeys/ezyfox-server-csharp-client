using System;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public interface EzyCodecCreator
	{
		Object newEncoder();

		Object newDecoder(int maxRequestSize);
	}
}
