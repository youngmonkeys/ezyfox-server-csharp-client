using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.io;
using com.tvd12.ezyfoxserver.client.codec;

namespace com.tvd12.ezyfoxserver.client.handler
{

	public interface EzyNioByteToObjectDecoder
	{
		Object decode(EzyMessage message);

		void decode(EzyByteBuffer bytes, Queue<EzyMessage> queue);
	}
}
