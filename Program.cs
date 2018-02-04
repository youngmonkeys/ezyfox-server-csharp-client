using System;
using System.Threading;
using com.tvd12.ezyfoxserver.client.api;
using com.tvd12.ezyfoxserver.client.evt;
using static com.tvd12.ezyfoxserver.client.evt.EzyEventType;

namespace com.tvd12.ezyfoxserver.client
{

	class MainClass
	{
		public static void Main(string[] args)
		{
			EzyClient client = new EzyClient();
			client.addEventHandler(CONNECTION_SUCCESS, new EzyConnectionSuccessEventHandler());
			client.connect("localhost", 3005);
			while (true)
			{
				Thread.Sleep(3);
				client.processEvents();
			}

		}
	}
}
