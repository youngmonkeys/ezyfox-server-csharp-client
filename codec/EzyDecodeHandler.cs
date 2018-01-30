using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.io;

namespace com.tvd12.ezyfoxserver.client.codec
{
	public interface EzyDecodeHandler
	{
		int nextState();

		EzyDecodeHandler nextHandler();

		bool handle(EzyByteBuffer input, Queue<EzyMessage> output);
	}
}
