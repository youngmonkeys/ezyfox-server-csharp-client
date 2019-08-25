using System;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySocketWritingLoopHandler : EzySocketEventLoopOneHandler
	{
		protected override String getThreadName()
		{
			return "ezyfox-socket-writer";
		}
	}

}
