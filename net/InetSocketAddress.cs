using System;
namespace com.tvd12.ezyfoxserver.client.net
{
	public class InetSocketAddress : SocketAddress
	{
		private readonly String host;
		private readonly int port;

		public InetSocketAddress(String host, int port)
		{
			this.host = host;
			this.port = port;
		}

		public override String getHost()
		{
			return this.host;
		}

		public override int getPort()
		{
			return this.port;
		}

		public override string ToString()
		{
			return string.Format("{0}:{1}", host, port);
		}
	}
}
