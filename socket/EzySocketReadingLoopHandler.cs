using System;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public class EzySocketReadingLoopHandler : EzySocketEventLoopOneHandler
	{
		protected override String getThreadName()
		{
			return "ezyfox-socket-reader";
		}
	}
}
