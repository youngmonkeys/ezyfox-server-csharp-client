using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.io;

namespace com.tvd12.ezyfoxserver.client.codec
{

	public interface EzyByteToObjectDecoder
	{
		Object decode(EzyMessage message);

		void decode(EzyByteBuffer bytes, Queue<EzyMessage> queue);
	}
}
