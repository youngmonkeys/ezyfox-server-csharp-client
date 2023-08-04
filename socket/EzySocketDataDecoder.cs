using System;
using com.tvd12.ezyfoxserver.client.callback;
using com.tvd12.ezyfoxserver.client.codec;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public interface EzySocketDataDecoder
	{
		Object decode(EzyMessage message, byte[] decryptionKey);

		void decode(byte[] bytes, EzyCallback<EzyMessage> callback);
	}
}
