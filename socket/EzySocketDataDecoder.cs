using System;
using com.tvd12.ezyfoxserver.client.callback;
using com.tvd12.ezyfoxserver.client.codec;
using com.tvd12.ezyfoxserver.client.util;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public interface EzySocketDataDecoder
	{
		Object decode(EzyMessage message);

		void decode(byte[] bytes, EzyCallback<EzyMessage> callback);
	}
}
