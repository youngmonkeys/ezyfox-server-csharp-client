using System;

namespace com.tvd12.ezyfoxserver.client.socket
{
	public sealed class EzySocketStatus
	{
		public const int CONNECTING 	= 1;
		public const int CONNECTED 		= 2;
		public const int DISCONNECTED 	= 3;

		private EzySocketStatus()
		{
		}
	}
}
