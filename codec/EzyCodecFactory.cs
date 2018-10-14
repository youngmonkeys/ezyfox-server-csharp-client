using System;
using com.tvd12.ezyfoxserver.client.constant;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public interface EzyCodecFactory
	{
		Object newEncoder(EzyConnectionType connectionType);

		Object newDecoder(EzyConnectionType connectionType);
	}
}
